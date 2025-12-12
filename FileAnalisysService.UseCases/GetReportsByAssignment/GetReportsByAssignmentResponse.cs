namespace FileAnalisysService.UseCases.GetReportsByAssignment;

public sealed record GetReportsByAssignmentResponse(
    Guid AssignmentId,
    IReadOnlyList<AssignmentWorkReportDto> Items
);