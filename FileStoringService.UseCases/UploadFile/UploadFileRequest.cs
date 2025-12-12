namespace FileStoringService.UseCases.UploadFile;

public sealed record UploadFileRequest
(
    string FileName,
    string? ContentType,
    long SizeInBytes,
    Stream Content
);

