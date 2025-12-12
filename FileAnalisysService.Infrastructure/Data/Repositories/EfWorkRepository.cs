using FileAnalisysService.Entities;
using FileAnalisysService.Infrastructure.Data.Dtos;
using FileAnalisysService.UseCases.SubmitWork;
using Microsoft.EntityFrameworkCore;

namespace FileAnalisysService.Infrastructure.Data.Repositories;

internal sealed class EfWorkRepository(FileAnalysisDbContext db) : IWorkRepository
{
    public void Add(Work work)
    {
        db.Works.Add(work.ToDto());
        db.SaveChanges();
    }

    public IReadOnlyList<Work> GetEarlierWorksByAssignment(Guid assignmentId, DateTimeOffset submittedAt)
    {
        return db.Works
            .AsNoTracking()
            .Where(x => x.AssignmentId == assignmentId && x.SubmittedAt < submittedAt)
            .OrderBy(x => x.SubmittedAt)
            .Select(x => x.ToEntity())
            .ToList();
    }
}