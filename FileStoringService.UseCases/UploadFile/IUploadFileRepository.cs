using FileStoringService.Entities;
namespace FileStoringService.UseCases.UploadFile;

public interface IUploadFileRepository
{
    void Add(StoredFile file);
}