namespace FileAnalisysService.Entities;

public sealed class Work
{
    public Guid Id { get; }
    public Guid StudentId { get; }
    public Guid AssignmentId { get; }
    public Guid FileId { get; }
    public DateTimeOffset SubmittedAt { get; }
    
    public Work(Guid id, Guid studentId, Guid assignmentId, Guid fileId, DateTimeOffset submittedAt)
    {
        if (studentId == Guid.Empty)
        {
            throw new ArgumentException("Идентификатор студента не задан.", nameof(studentId));
        }

        if (assignmentId == Guid.Empty)
        {
            throw new ArgumentException("Идентификатор задания не задан.", nameof(assignmentId));
        }

        if (fileId == Guid.Empty)
        {
            throw new ArgumentException("Идентификатор файла не задан.", nameof(fileId)); 
        }
        Id = id;
        StudentId = studentId;
        AssignmentId = assignmentId;
        FileId = fileId;
        SubmittedAt =  submittedAt;
    }
}