namespace FileAnalisysService.UseCases.SubmitWork;

public sealed record SubmitWorkRequest(
    Guid StudentId,
    Guid AssignmentId,
    Guid FileId
);