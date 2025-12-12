namespace FileStoringService.UseCases.DownloadFile;

public sealed record DownloadFileResponse(
    Stream Content,
    string ContentType,
    string FileName,
    string ContentHash
);