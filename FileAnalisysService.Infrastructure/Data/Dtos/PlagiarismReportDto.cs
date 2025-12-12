using FileAnalisysService.Entities;

namespace FileAnalisysService.Infrastructure.Data.Dtos;

internal sealed class PlagiarismReportDto
{
    public Guid Id { get; set; }
    public Guid WorkId { get; set; }

    public ReportStatus Status { get; set; }

    public bool? IsPlagiarism { get; set; }
    public string? Details { get; set; }
    public Guid? OriginalWorkId { get; set; }
}