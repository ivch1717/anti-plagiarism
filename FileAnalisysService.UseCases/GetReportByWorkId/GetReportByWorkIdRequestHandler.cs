namespace FileAnalisysService.UseCases.GetReportByWorkId;

public class GetReportByWorkIdRequestHandler : IGetReportByWorkIdRequestHandler
{
    private readonly IGetReportByWorkIdRepository _repository;

    public GetReportByWorkIdRequestHandler(
        IGetReportByWorkIdRepository repository)
    {
        _repository = repository;
    }
    
    public GetReportByWorkIdResponse Handle(GetReportsByWorkIdRequest request)
    {
        if (request.WorkId == Guid.Empty)
            throw new ArgumentException("Некорректный идентификатор работы");

        var report = _repository.GetByWorkId(request.WorkId);

        if (report is null)
            throw new KeyNotFoundException("Отчёт по работе не найден");

        return report.ToResponse();
    }
}