namespace FileAnalisysService.UseCases.GetReportsByAssignment;


public interface IGetReportsByAssignmentRequestHandler
{
    GetReportsByAssignmentResponse Handle(GetReportsByAssignmentRequest request);
}