namespace FileAnalisysService.UseCases.Ports;

public interface IFileStoringClient
{
    string GetFileHash(Guid fileId);
    FileDownloadDto Download(Guid fileId);
}