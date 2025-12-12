namespace FileAnalisysService.UseCases.GetReportsByWorkId;

public interface IGetReportByWorkIdRequestHandler
{ 
    GetReportByWorkIdResponse Handle(GetReportsByWorkIdRequest request);
}