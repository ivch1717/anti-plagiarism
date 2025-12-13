using FileAnalisysService.Entities;
namespace FileAnalisysService.UseCases.GetReportByWorkId;

public interface IGetReportByWorkIdRepository
{
    PlagiarismReport? GetByWorkId(Guid workId);
}