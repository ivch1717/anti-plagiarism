using FileAnalisysService.Entities;
using FileStoringService.UseCases.DownloadFile;

namespace FileAnalisysService.UseCases.SubmitWork;

public class SubmitWorkRequestHandler : ISubmitWorkRequestHandler
{
    private readonly IWorkRepository _workRepository;
    private readonly IPlagiarismReportRepository _reportRepository;
    private readonly IDownloadFileRequestHandler _downloadFileRequestHandler;

    public SubmitWorkRequestHandler(
        IWorkRepository workRepository,
        IPlagiarismReportRepository reportRepository,
        IDownloadFileRequestHandler downloadFileRequestHandler)
    {
        _workRepository = workRepository;
        _reportRepository = reportRepository;
        _downloadFileRequestHandler = downloadFileRequestHandler;
    }

    public SubmitWorkResponse Handle(SubmitWorkRequest request)
    {
        if (request.StudentId == Guid.Empty)
            throw new ArgumentException("StudentId не задан");

        if (request.AssignmentId == Guid.Empty)
            throw new ArgumentException("AssignmentId не задан");

        if (request.FileId == Guid.Empty)
            throw new ArgumentException("FileId не задан");

        var workId = Guid.NewGuid();
        var submittedAt = DateTimeOffset.UtcNow;

        var work = request.ToEntity(workId, submittedAt);

        _workRepository.Add(work);

        var reportId = Guid.NewGuid();
        PlagiarismReport report = SubmitWorkMapper.ToEntity(reportId, workId);

        _reportRepository.Add(report);
        
        try
        {
            var earlierWorks = _workRepository
                .GetEarlierWorksByAssignment(request.AssignmentId, submittedAt);
            
            var originalWork = earlierWorks
                .FirstOrDefault(w => w.StudentId != request.StudentId && _downloadFileRequestHandler.Handle(w.StudentId).ContentHash
                                      == _downloadFileRequestHandler.Handle(request.StudentId).ContentHash);

            if (originalWork != null)
            {
                report.MarkCompleted(
                    isPlagiarism: true,
                    originalWorkId: originalWork.Id,
                    details: "Найдена более ранняя сдача по данному заданию"
                );
            }
            else
            {
                report.MarkCompleted(
                    isPlagiarism: false,
                    originalWorkId: null,
                    details: "Заимствований не обнаружено"
                );
            }
        }
        catch (Exception ex)
        {
            report.MarkFailed(ex.Message);
        }

        _reportRepository.Update(report);

        return SubmitWorkMapper.ToResponse(work.Id, report.Id, report.Status);
    }
}