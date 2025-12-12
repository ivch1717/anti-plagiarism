using FileStoringService.Entities;
using FileStoringService.UseCases.UploadFile;

namespace FileStoringService.Infrastructure.Data.Repositories;

internal sealed class EfUploadFileRepository(FileStoringDbContext dbContext) : IUploadFileRepository
{
    public void Add(StoredFile file)
    {
        dbContext.StoredFiles.Add(file.ToDto());
        dbContext.SaveChanges();
    }
}