namespace FileAnalisysService.UseCases.GetReportByWorkId;

public sealed record GetReportsByWorkIdRequest(
    Guid WorkId
);