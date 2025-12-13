namespace FileStoringService.UseCases.GetFileHash;

public interface IGetFileHashRequestHandler
{
    GetFileHashResponse Handle(GetFileHashRequest request);
}