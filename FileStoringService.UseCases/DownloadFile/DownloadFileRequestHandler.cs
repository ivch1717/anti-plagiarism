
using FileStoringService.UseCases.UploadFile;
namespace FileStoringService.UseCases.DownloadFile;

public class DownloadFileRequestHandler : IDownloadFileRequestHandler
{
    private readonly IDownloadFileRepository _repository;
    private readonly IFileStorage _fileStorage;

    public DownloadFileRequestHandler(
        IDownloadFileRepository repository,
        IFileStorage fileStorage)
    {
        _repository = repository;
        _fileStorage = fileStorage;
    }
    
    
    public DownloadFileResponse Handle(DownloadFileRequest request)
    {
        if (request.FileId == Guid.Empty)
            throw new ArgumentException("Некорректный идентификатор файла");

        var storedFile = _repository.GetById(request.FileId)
                         ?? throw new FileNotFoundException("Файл не найден");

        var stream = _fileStorage.OpenRead(storedFile.StorageKey);

        return storedFile.ToDto(stream);
    }

}