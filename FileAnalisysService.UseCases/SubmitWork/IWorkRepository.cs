using FileAnalisysService.Entities;
namespace FileAnalisysService.UseCases.SubmitWork;

public interface IWorkRepository
{
    void Add(Work work);

    IReadOnlyList<Work> GetEarlierWorksByAssignment(
        Guid assignmentId,
        DateTimeOffset submittedAt
    );
}