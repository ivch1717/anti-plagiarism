using FileAnalisysService.Entities;
namespace FileAnalisysService.UseCases.GetReportsByWorkId;

public interface IGetReportByWorkIdRepository
{
    PlagiarismReport? GetByWorkId(Guid workId);
}