namespace FileAnalisysService.UseCases.GetReportsByWorkId;

public sealed record GetReportByWorkIdResponse(
    Guid ReportId,
    Guid WorkId,
    Entities.ReportStatus Status,
    bool? IsPlagiarism,
    Guid? OriginalWorkId,
    string? Details
);