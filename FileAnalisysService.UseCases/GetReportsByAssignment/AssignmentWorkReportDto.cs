namespace FileAnalisysService.UseCases.GetReportsByAssignment;

public sealed record AssignmentWorkReportDto(
    Guid WorkId,
    Guid StudentId,
    DateTimeOffset SubmittedAt,
    Guid FileId,
    Guid ReportId,
    Entities.ReportStatus Status,
    bool? IsPlagiarism,
    Guid? OriginalWorkId,
    string? Details
);