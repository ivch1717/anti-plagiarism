using FileStoringService.Infrastructure.Data;
using FileStoringService.Infrastructure.Data.Repositories;
using FileStoringService.Infrastructure.Storage;
using FileStoringService.UseCases.DownloadFile;
using FileStoringService.UseCases.UploadFile;
using FileStoringService.UseCases.GetFileHash;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileStoringService.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFileStoringInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<FileStoringDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"));
        });
        
        services.AddScoped<IUploadFileRepository, EfUploadFileRepository>();
        services.AddScoped<IDownloadFileRepository, EfDownloadFileRepository>();
        services.AddScoped<IGetFileHashRepository, EfGetFileHashRepository>();
        
        services.AddSingleton<IFileStorageUploadFile, LocalFileStorage>();
        services.AddSingleton<IFileStoragelDownloadFile, LocalFileStorage>();
        
        services.AddHostedService<MigrationRunner>();

        return services;
    }
}