namespace FileAnalisysService.UseCases.GetReportsByAssignment;

public class GetReportsByAssignmentRequestHandler : IGetReportsByAssignmentRequestHandler
{
    private readonly IGetReportsByAssignmentRepository _repository;

    public GetReportsByAssignmentRequestHandler(IGetReportsByAssignmentRepository repository)
    {
        _repository = repository;
    }

    public GetReportsByAssignmentResponse Handle(GetReportsByAssignmentRequest request)
    {
        if (request.AssignmentId == Guid.Empty)
            throw new ArgumentException("Некорректный идентификатор задания");

        var rows = _repository.GetByAssignmentId(request.AssignmentId);

        return GetReportsByAssignmentMapper.ToResponse(request.AssignmentId, rows);
    }
}