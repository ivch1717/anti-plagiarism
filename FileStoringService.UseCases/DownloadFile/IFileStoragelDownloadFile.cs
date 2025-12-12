namespace FileStoringService.UseCases.DownloadFile;

public interface IFileStoragelDownloadFile
{
    Stream OpenRead(string storageKey);
}