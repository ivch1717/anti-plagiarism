namespace FileStoringService.UseCases.DownloadFile;

public interface IDownloadFileRequestHandler
{
    DownloadFileResponse Handle(DownloadFileRequest request);
}