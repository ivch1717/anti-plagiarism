namespace FileStoringService.Entities;

public sealed class StoredFile
{
    public Guid Id { get; }
    public string StorageKey { get; }      
    public string FileName { get; }
    public string ContentType { get; }   
    public long SizeInBytes { get; }
    public string ContentHash { get; }

    public StoredFile(
        Guid id,
        string storageKey,
        string fileName,
        string contentType,
        long sizeInBytes,
        string contentHash
        )
    {
        if (string.IsNullOrWhiteSpace(storageKey))
        {
            throw new ArgumentException("Путь к файлу не может быть пустым");
        }

        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("Название файла не может быть пустым");
        }

        if (sizeInBytes < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sizeInBytes), "Размер файла не может быть отрицательным числом");
        }

        if (string.IsNullOrWhiteSpace(contentHash))
        {
            throw new ArgumentException("Хэш содержимого файла не может быть пустым.");
        }
        
        Id = id;
        StorageKey = storageKey;
        FileName = fileName;
        ContentType = contentType;
        SizeInBytes = sizeInBytes;
        ContentHash = contentHash;
    }
}