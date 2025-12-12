using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FileStoringService.Infrastructure.Data;

internal sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FileStoringDbContext>
{
    public FileStoringDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<FileStoringDbContext>()
            .UseNpgsql("Server=127.0.0.1;Port=5432;Database=filestoring;User Id=postgres;Password=postgres;")
            .Options;

        return new FileStoringDbContext(options);
    }
}