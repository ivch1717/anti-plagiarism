using FileStoringService.Entities;

namespace FileStoringService.UseCases.GetFileHash;

public interface IGetFileHashRepository
{
    StoredFile? GetById(Guid id);
}