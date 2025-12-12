namespace FileStoringService.UseCases.UploadFile;

public interface IUploadFileRequestHandler
{
    UploadFileResponse Handle(UploadFileRequest request);
}