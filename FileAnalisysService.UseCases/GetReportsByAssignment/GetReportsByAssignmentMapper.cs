using FileAnalisysService.Entities;

namespace FileAnalisysService.UseCases.GetReportsByAssignment;

internal static class GetReportsByAssignmentMapper
{
    public static AssignmentWorkReportDto ToDto(this (Work Work, PlagiarismReport Report) x)
    {
        var work = x.Work;
        var report = x.Report;

        return new AssignmentWorkReportDto(
            WorkId: work.Id,
            StudentId: work.StudentId,
            SubmittedAt: work.SubmittedAt,
            FileId: work.FileId,
            ReportId: report.Id,
            Status: report.Status,
            IsPlagiarism: report.IsPlagiarism,
            OriginalWorkId: report.OriginalWorkId,
            Details: report.Details
        );
    }

    public static GetReportsByAssignmentResponse ToResponse(
        Guid assignmentId,
        IReadOnlyList<(Work Work, PlagiarismReport Report)> rows)
    {
        var items = rows
            .Select(r => r.ToDto())
            .OrderBy(x => x.SubmittedAt)
            .ToList();

        return new GetReportsByAssignmentResponse(
            AssignmentId: assignmentId,
            Items: items
        );
    }
}