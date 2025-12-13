using FileAnalisysService.Entities;
using FileAnalisysService.UseCases.Ports;
using FileAnalisysService.UseCases.SubmitWork;

namespace FileAnalisysService.UseCases.SubmitWork;

public class SubmitWorkRequestHandler : ISubmitWorkRequestHandler
{
    private readonly IWorkRepository _workRepository;
    private readonly IPlagiarismReportRepository _reportRepository;
    private readonly IFileStoringClient _fileStoringClient;

    public SubmitWorkRequestHandler(
        IWorkRepository workRepository,
        IPlagiarismReportRepository reportRepository,
        IFileStoringClient fileStoringClient)
    {
        _workRepository = workRepository;
        _reportRepository = reportRepository;
        _fileStoringClient = fileStoringClient;
    }

    public SubmitWorkResponse Handle(SubmitWorkRequest request)
    {
        if (request.StudentId == Guid.Empty)
            throw new ArgumentException("StudentId не задан");

        if (request.AssignmentId == Guid.Empty)
            throw new ArgumentException("AssignmentId не задан");

        if (request.FileId == Guid.Empty)
            throw new ArgumentException("FileId не задан");

        var submittedAt = DateTimeOffset.UtcNow;
        var workId = Guid.NewGuid();

        var currentFileHash = _fileStoringClient.GetFileHash(request.FileId);

        var work = request.ToEntity(workId, submittedAt);
        _workRepository.Add(work);

        var reportId = Guid.NewGuid();
        var report = SubmitWorkMapper.ToEntity(reportId, workId);

        try
        {
            var earlierWorks = _workRepository.GetEarlierWorksByAssignment(request.AssignmentId, submittedAt);

            Work? originalWork = null;

            foreach (var earlierWork in earlierWorks)
            {
                if (earlierWork.StudentId == request.StudentId)
                    continue;

                var earlierFileHash = _fileStoringClient.GetFileHash(earlierWork.FileId);

                if (earlierFileHash == currentFileHash)
                {
                    originalWork = earlierWork;
                    break;
                }
            }

            if (originalWork != null)
                report.MarkCompleted(true, originalWork.Id, "Обнаружено совпадение по хешу файла");
            else
                report.MarkCompleted(false, null, "Совпадений не найдено");
        }
        catch (Exception ex)
        {
            report.MarkFailed(ex.Message);
        }

        _reportRepository.Add(report);

        return SubmitWorkMapper.ToResponse(work.Id, report.Id, report.Status);

    }
}
