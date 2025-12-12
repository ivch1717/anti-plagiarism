using FileAnalisysService.Entities;
namespace FileAnalisysService.UseCases.SubmitWork;

internal static class SubmitWorkMapper
{
    public static Work ToEntity(
        this SubmitWorkRequest request,
        Guid workId,
        DateTimeOffset submittedAt)
    {
        return new Work(
            id: workId,
            studentId: request.StudentId,
            assignmentId: request.AssignmentId,
            fileId: request.FileId,
            submittedAt: submittedAt
        );
    }
    
    public static PlagiarismReport ToEntity(Guid reportId, Guid workId)
    {
        return new PlagiarismReport(
            id: reportId,
            workId: workId
        );
    }
    
    public static SubmitWorkResponse ToResponse(
        Guid workId,
        Guid reportId, 
        Entities.ReportStatus status)
    {
        return new SubmitWorkResponse(
            WorkId: workId,
            ReportId: reportId,
            Status: status
        );
    }
}