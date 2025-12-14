# HSE-HW-3
## Синхронное межсервисное взаимодействие


## Начало работы 
``` bash
docker-compose build
docker-compose up -d
```

## Завершение работы
``` bash
docker-compose down
```

# Документация

API Gateway (единая точка входа):
- http://localhost:9000/swagger

File Storing Service:
- http://localhost:5158/swagger

File Analysis Service:
- http://localhost:5160/swagger

> Swagger добавлен во все 3 сервиса. В API Gateway Swagger показывает публичные эндпоинты, которые проксируют запросы в микросервисы.

---

## Архитектура

Система построена по микросервисной архитектуре и состоит из трех сервисов:

1) **File Storing Service** (`5158`)
- отвечает только за хранение и выдачу файлов
- основные операции:
  - `POST /files` — загрузка файла
  - `GET /files/{fileId}` — скачать файл
  - `GET /files/{fileId}/hash` — получить хеш содержимого (SHA-256)

2) **File Analysis Service** (`5160`)
- отвечает за фиксацию сдачи работы, анализ на плагиат, хранение отчетов и выдачу отчетов
- основные операции:
  - `POST /works` — зафиксировать сдачу работы (studentId, assignmentId, fileId), создать отчет
  - `GET /works/{workId}/report` — получить отчет по конкретной работе
  - `GET /assignments/{assignmentId}/reports` — получить отчеты по заданию (assignment)

3) **API Gateway** (`9000`)
- центральный сервис-посредник
- принимает запросы клиентов и маршрутизирует их в нужный микросервис
- единый публичный API:
  - `POST /works` (multipart/form-data: file + studentId + assignmentId)
  - `GET /files/{fileId}`
  - `GET /files/{fileId}/hash`
  - `GET /works/{workId}/report`
  - `GET /assignments/{assignmentId}/reports`

---

## Docker

Все микросервисы упакованы в Docker-контейнеры. Вся система разворачивается одной командой:

Порты:

* File Storing Service: `5158:8080`
* File Analysis Service: `5160:8080`
* API Gateway: `9000:8080`

---

## Пользовательские сценарии и взаимодействие сервисов

### 1) Студент отправляет работу на проверку (основной сценарий)

**Цель:** загрузить файл + указать studentId/assignmentId, получить workId/reportId и fileId.

**Запрос клиента (в Gateway):**

* `POST /works`
* `multipart/form-data`:

  * `studentId` (Guid)
  * `assignmentId` (Guid)
  * `file` (файл)

**Технический сценарий обмена данными:**

1. **API Gateway → File Storing Service**

   * Gateway отправляет файл в `POST /files`
   * File Storing сохраняет файл (локально/в файловое хранилище) и записывает метаданные
   * Возвращает `fileId`

2. **API Gateway → File Analysis Service**

   * Gateway вызывает `POST /works` и передает:

     * `studentId`, `assignmentId`, `fileId`
   * File Analysis:

     * фиксирует сдачу работы в БД (кто/когда/какое задание/какой файл)
     * запускает проверку на плагиат по алгоритму ниже
     * создает/обновляет отчет и сохраняет его

3. **Ответ Gateway клиенту**

   * Gateway возвращает агрегированный ответ:

   ```json
   {
     "fileId": "...",
     "workId": "...",
     "reportId": "...",
     "status": 0
   }
   ```

---

### 2) Преподаватель получает отчет по конкретной работе

**Запрос клиента (в Gateway):**

* `GET /works/{workId}/report`

**Сценарий:**

1. Gateway → File Analysis: `GET /works/{workId}/report`
2. File Analysis возвращает отчет (статус, флаг плагиата, originalWorkId, details)
3. Gateway возвращает ответ клиенту “как есть”

---

### 3) Преподаватель получает отчеты по заданию

**Запрос клиента (в Gateway):**

* `GET /assignments/{assignmentId}/reports`

**Сценарий:**

1. Gateway → File Analysis: `GET /assignments/{assignmentId}/reports`
2. File Analysis возвращает список пар (Work + Report) по заданию
3. Gateway отдает ответ клиенту

---

### 4) Скачать исходный файл

**Запрос клиента (в Gateway):**

* `GET /files/{fileId}`

**Сценарий:**

1. Gateway → File Storing: `GET /files/{fileId}` (stream)
2. Gateway проксирует поток в ответ клиенту
3. Клиент получает файл как скачивание (Content-Disposition attachment)

---

## Алгоритм определения признаков плагиата (MVP)

В MVP считается, что **плагиат присутствует**, если существует **более ранняя сдача другим студентом** того же задания, у которой **совпадает содержимое файла**.

Алгоритм в File Analysis Service при `POST /works`:

1. Получить `studentId`, `assignmentId`, `fileId`.
2. Сохранить факт сдачи работы в БД (Work):

   * `Id`, `StudentId`, `AssignmentId`, `SubmittedAt`, `FileId`
3. Получить хеш файла из File Storing Service:

   * `GET /files/{fileId}/hash` → `hash`
4. Найти более ранние работы по этому же `assignmentId`:

   * `GetEarlierWorksByAssignment(assignmentId, submittedAt)`
5. Для каждой более ранней работы:

   * получить хеш ее файла (через File Storing)
   * сравнить с текущим хешем
6. Если найдено совпадение хеша и студент другой:

   * `IsPlagiarism = true`
   * `OriginalWorkId = <id более ранней работы>`
   * `Details = "Совпадение хеша файла с более ранней сдачей"`
7. Если совпадений нет:

   * `IsPlagiarism = false`
   * `OriginalWorkId = null`
   * `Details = "Совпадений не найдено"`
8. Сохранить отчет (PlagiarismReport) в БД.

---

## Обработка ошибок (если сервис недоступен)

API Gateway является фасадом и обрабатывает типовые сбои синхронного взаимодействия:

* если **File Storing Service** недоступен/таймаут:

  * `POST /works` и `GET /files/*` возвращают `503 Service Unavailable`
* если **File Analysis Service** недоступен/таймаут:

  * `POST /works`, `GET /works/*`, `GET /assignments/*` возвращают `503 Service Unavailable`

Это демонстрирует поведение системы при падении одного из микросервисов (требование задания по обработке ошибок).

---

## Примеры запросов (через Gateway)

1. Отправка работы:

* `POST http://localhost:9000/works`
* form-data:

  * studentId: `<guid>`
  * assignmentId: `<guid>`
  * file: `<file>`

2. Скачать файл:

* `GET http://localhost:9000/files/{fileId}`

3. Получить отчет по workId:

* `GET http://localhost:9000/works/{workId}/report`

4. Получить отчеты по assignmentId:

* `GET http://localhost:9000/assignments/{assignmentId}/reports`

```
::contentReference[oaicite:0]{index=0}
```

