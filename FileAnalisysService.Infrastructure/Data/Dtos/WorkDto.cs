namespace FileAnalisysService.Infrastructure.Data.Dtos;

internal sealed class WorkDto
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Guid AssignmentId { get; set; }
    public Guid FileId { get; set; }
    public DateTimeOffset SubmittedAt { get; set; }
}