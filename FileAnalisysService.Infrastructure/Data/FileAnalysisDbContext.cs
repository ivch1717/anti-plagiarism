using FileAnalisysService.Infrastructure.Data.Dtos;
using Microsoft.EntityFrameworkCore;

namespace FileAnalisysService.Infrastructure.Data;

internal sealed class FileAnalysisDbContext(DbContextOptions<FileAnalysisDbContext> options) : DbContext(options)
{
    public DbSet<WorkDto> Works => Set<WorkDto>();
    public DbSet<PlagiarismReportDto> PlagiarismReports => Set<PlagiarismReportDto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkDto>(builder =>
        {
            builder.ToTable("Works");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.StudentId).IsRequired();
            builder.Property(x => x.AssignmentId).IsRequired();
            builder.Property(x => x.FileId).IsRequired();
            builder.Property(x => x.SubmittedAt).IsRequired();

            builder.HasIndex(x => x.AssignmentId);
            builder.HasIndex(x => new { x.AssignmentId, x.SubmittedAt });
        });

        modelBuilder.Entity<PlagiarismReportDto>(builder =>
        {
            builder.ToTable("PlagiarismReports");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.WorkId).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.HasIndex(x => x.WorkId).IsUnique();
            builder.Property(x => x.IsPlagiarism);  
            builder.Property(x => x.Details);           
            builder.Property(x => x.OriginalWorkId);
            
            builder.HasIndex(x => x.WorkId).IsUnique();
            builder.HasIndex(x => x.Status);
        });
    }
}