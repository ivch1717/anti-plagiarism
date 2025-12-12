using FileAnalisysService.Entities;
using FileAnalisysService.Infrastructure.Data.Dtos;

namespace FileAnalisysService.Infrastructure.Data;

internal static class DataMapper
{
    public static WorkDto ToDto(this Work entity)
    {
        return new WorkDto
        {
            Id = entity.Id,
            StudentId = entity.StudentId,
            AssignmentId = entity.AssignmentId,
            FileId = entity.FileId,
            SubmittedAt = entity.SubmittedAt
        };
    }

    public static Work ToEntity(this WorkDto dto)
    {
        return new Work(
            id: dto.Id,
            studentId: dto.StudentId,
            assignmentId: dto.AssignmentId,
            fileId: dto.FileId,
            submittedAt: dto.SubmittedAt
        );
    }

    public static PlagiarismReportDto ToDto(this PlagiarismReport entity)
    {
        return new PlagiarismReportDto
        {
            Id = entity.Id,
            WorkId = entity.WorkId,
            Status = entity.Status,
            IsPlagiarism = entity.IsPlagiarism,
            Details = entity.Details,
            OriginalWorkId = entity.OriginalWorkId
        };
    }

    public static PlagiarismReport ToEntity(this PlagiarismReportDto dto)
    {
        var report = new PlagiarismReport(dto.Id, dto.WorkId);
        
        if (dto.Status == ReportStatus.Completed)
        {
            report.MarkCompleted(
                isPlagiarism: dto.IsPlagiarism ?? false,
                originalWorkId: dto.OriginalWorkId,
                details: dto.Details
            );
        }
        else if (dto.Status == ReportStatus.Failed)
        {
            report.MarkFailed(dto.Details);
        }

        return report;
    }
}