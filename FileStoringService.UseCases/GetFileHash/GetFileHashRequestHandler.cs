using FileStoringService.Entities;

namespace FileStoringService.UseCases.GetFileHash;

public class GetFileHashRequestHandler : IGetFileHashRequestHandler
{
    private readonly IGetFileHashRepository _repository;

    public GetFileHashRequestHandler(IGetFileHashRepository repository)
    {
        _repository = repository;
    }

    public GetFileHashResponse Handle(GetFileHashRequest request)
    {
        if (request.FileId == Guid.Empty)
            throw new ArgumentException("Некорректный идентификатор файла");

        StoredFile storedFile = _repository.GetById(request.FileId)
                                ?? throw new FileNotFoundException("Файл не найден");

        return storedFile.ToDto();
    }
}