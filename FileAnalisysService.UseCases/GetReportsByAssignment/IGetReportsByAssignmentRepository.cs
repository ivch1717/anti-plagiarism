using FileAnalisysService.Entities;
namespace FileAnalisysService.UseCases.GetReportsByAssignment;

public interface IGetReportsByAssignmentRepository
{
    IReadOnlyList<(Work Work, PlagiarismReport Report)> GetByAssignmentId(Guid assignmentId);
}