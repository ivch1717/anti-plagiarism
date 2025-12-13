using FileAnalisysService.Entities;
namespace FileAnalisysService.UseCases.GetReportByWorkId;

internal static class GetReportByWorkIdMapper
{
    public static GetReportByWorkIdResponse ToResponse(this PlagiarismReport report)
    {
        return new GetReportByWorkIdResponse(
            ReportId: report.Id,
            WorkId: report.WorkId,
            Status: report.Status,
            IsPlagiarism: report.IsPlagiarism,
            OriginalWorkId: report.OriginalWorkId,
            Details: report.Details
        );
    }
}