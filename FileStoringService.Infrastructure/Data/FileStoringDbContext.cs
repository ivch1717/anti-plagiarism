using FileStoringService.Infrastructure.Data.Dtos;
using Microsoft.EntityFrameworkCore;


namespace FileStoringService.Infrastructure.Data;

internal sealed class FileStoringDbContext(DbContextOptions<FileStoringDbContext> options) : DbContext(options)
{
    public DbSet<StoredFileDto> StoredFiles => Set<StoredFileDto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StoredFileDto>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("StoredFiles");
            builder.Property(x => x.StorageKey).IsRequired();
            builder.Property(x => x.FileName).IsRequired();
            builder.Property(x => x.ContentType).IsRequired();
            builder.Property(x => x.ContentHash).IsRequired();
            builder.Property(x => x.SizeInBytes).IsRequired();

            builder.HasIndex(x => x.ContentHash);
        });
    }
}