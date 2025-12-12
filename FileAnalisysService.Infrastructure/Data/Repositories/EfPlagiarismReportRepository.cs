using FileAnalisysService.Entities;
using FileAnalisysService.UseCases.GetReportsByWorkId;
using FileAnalisysService.UseCases.SubmitWork;
using Microsoft.EntityFrameworkCore;

namespace FileAnalisysService.Infrastructure.Data.Repositories;

internal sealed class EfPlagiarismReportRepository(FileAnalysisDbContext db)
    : IPlagiarismReportRepository, IGetReportByWorkIdRepository
{
    public void Add(PlagiarismReport report)
    {
        db.PlagiarismReports.Add(report.ToDto());
        db.SaveChanges();
    }

    public void Update(PlagiarismReport report)
    {
        var dto = db.PlagiarismReports.FirstOrDefault(x => x.Id == report.Id);
        if (dto is null)
            throw new InvalidOperationException("Отчёт для обновления не найден");

        dto.Status = report.Status;
        dto.IsPlagiarism = report.IsPlagiarism;
        dto.Details = report.Details;
        dto.OriginalWorkId = report.OriginalWorkId;

        db.SaveChanges();
    }

    public PlagiarismReport? GetByWorkId(Guid workId)
    {
        var dto = db.PlagiarismReports
            .AsNoTracking()
            .FirstOrDefault(x => x.WorkId == workId);

        return dto?.ToEntity();
    }
}