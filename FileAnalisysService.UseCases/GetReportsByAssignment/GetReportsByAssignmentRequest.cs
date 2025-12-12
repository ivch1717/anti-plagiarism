namespace FileAnalisysService.UseCases.GetReportsByAssignment;

public sealed record GetReportsByAssignmentRequest(
    Guid AssignmentId
);