namespace FileAnalisysService.Entities;

public class PlagiarismReport
{
    public Guid Id { get; }
    public Guid WorkId { get; }
    public ReportStatus Status { get; private set; }
    
    public bool? IsPlagiarism { get; private set; }
    public string? Details { get; private set; }
    public Guid? OriginalWorkId { get; private set; }

    public PlagiarismReport(Guid id, Guid workId)
    {
        if (workId == Guid.Empty)
        {
            throw new ArgumentException("Идентификатор работы не задан.", nameof(workId));
        }
        
        Id = id;
        WorkId = workId;
        Status = ReportStatus.Pending;
    }

    public void MarkCompleted(
        bool isPlagiarism,
        Guid? originalWorkId,
        string? details
    )
    {
        Status = ReportStatus.Completed;
        IsPlagiarism = isPlagiarism;
        OriginalWorkId = originalWorkId;
        Details = details;
    }
    
    public void MarkFailed(string? details)
    {
        Status = ReportStatus.Failed;
        Details = details;
    }
    
}