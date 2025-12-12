namespace FileAnalisysService.UseCases.SubmitWork;

public sealed record SubmitWorkResponse(
    Guid WorkId,
    Guid ReportId,
    Entities.ReportStatus Status
);