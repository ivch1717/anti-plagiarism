using FileAnalisysService.Infrastructure.Data;
using FileAnalisysService.Infrastructure.Data.Repositories;
using FileAnalisysService.UseCases.GetReportByWorkId;
using FileAnalisysService.UseCases.GetReportsByAssignment;
using FileAnalisysService.UseCases.SubmitWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileAnalisysService.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFileAnalysisInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<FileAnalysisDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"));
        });
        
        services.AddScoped<IWorkRepository, EfWorkRepository>();
        services.AddScoped<IPlagiarismReportRepository, EfPlagiarismReportRepository>();
        
        services.AddScoped<IGetReportByWorkIdRepository, EfPlagiarismReportRepository>();
        services.AddScoped<IGetReportsByAssignmentRepository, EfGetReportsByAssignmentRepository>();
        
        services.AddHostedService<MigrationRunner>();

        return services;
    }
}