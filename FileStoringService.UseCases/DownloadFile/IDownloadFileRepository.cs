using FileStoringService.Entities;
namespace FileStoringService.UseCases.DownloadFile;

public interface IDownloadFileRepository
{
    StoredFile? GetById(Guid id);
}