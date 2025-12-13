using FileStoringService.Entities;
using FileStoringService.UseCases.GetFileHash;
using Microsoft.EntityFrameworkCore;

namespace FileStoringService.Infrastructure.Data.Repositories;

internal sealed class EfGetFileHashRepository(FileStoringDbContext dbContext) : IGetFileHashRepository
{
    public StoredFile? GetById(Guid id)
    {
        var dto = dbContext.StoredFiles
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);

        return dto?.ToEntity();
    }
}