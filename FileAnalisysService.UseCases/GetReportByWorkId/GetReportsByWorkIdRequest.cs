namespace FileAnalisysService.UseCases.GetReportsByWorkId;

public sealed record GetReportsByWorkIdRequest(
    Guid WorkId
);