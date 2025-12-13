namespace FileAnalisysService.UseCases.SubmitWork;

public interface IFileStoringClient
{
    string GetFileHash(Guid fileId);
}