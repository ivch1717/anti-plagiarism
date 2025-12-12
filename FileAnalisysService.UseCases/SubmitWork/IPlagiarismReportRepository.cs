using FileAnalisysService.Entities;
namespace FileAnalisysService.UseCases.SubmitWork;

public interface IPlagiarismReportRepository
{
    void Add(PlagiarismReport report);
    void Update(PlagiarismReport report);
}