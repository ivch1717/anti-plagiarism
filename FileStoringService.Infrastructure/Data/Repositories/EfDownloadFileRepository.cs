using FileStoringService.Entities;
using FileStoringService.UseCases.DownloadFile;
using Microsoft.EntityFrameworkCore;

namespace FileStoringService.Infrastructure.Data.Repositories;

internal sealed class EfDownloadFileRepository(FileStoringDbContext dbContext) : IDownloadFileRepository
{
    public StoredFile? GetById(Guid id)
    {
        var dto = dbContext.StoredFiles
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);

        return dto?.ToEntity();
    }
}