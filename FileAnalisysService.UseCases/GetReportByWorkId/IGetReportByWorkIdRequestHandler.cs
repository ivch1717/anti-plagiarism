namespace FileAnalisysService.UseCases.GetReportByWorkId;

public interface IGetReportByWorkIdRequestHandler
{ 
    GetReportByWorkIdResponse Handle(GetReportsByWorkIdRequest request);
}