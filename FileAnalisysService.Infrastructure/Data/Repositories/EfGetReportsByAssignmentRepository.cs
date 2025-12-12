using FileAnalisysService.Entities;
using FileAnalisysService.UseCases.GetReportsByAssignment;
using Microsoft.EntityFrameworkCore;

namespace FileAnalisysService.Infrastructure.Data.Repositories;

internal sealed class EfGetReportsByAssignmentRepository(FileAnalysisDbContext db)
    : IGetReportsByAssignmentRepository
{
    public IReadOnlyList<(Work Work, PlagiarismReport Report)> GetByAssignmentId(Guid assignmentId)
    {
        var rows = (
            from w in db.Works.AsNoTracking()
            join r in db.PlagiarismReports.AsNoTracking()
                on w.Id equals r.WorkId
            where w.AssignmentId == assignmentId
            select new { Work = w, Report = r }
        ).ToList();

        return rows
            .Select(x => (x.Work.ToEntity(), x.Report.ToEntity()))
            .ToList();
    }
}