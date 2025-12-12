using FileStoringService.Entities;
namespace FileStoringService.UseCases.DownloadFile;

internal static class DownloadFileMapper
{
    public static DownloadFileResponse ToDto(this StoredFile file, Stream content)
    {
        return new DownloadFileResponse(
            Content: content,
            ContentType: file.ContentType,
            FileName: file.FileName,
            ContentHash: file.ContentHash
        );
    }
}