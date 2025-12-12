using FileStoringService.Entities;
namespace FileStoringService.UseCases.UploadFile;

internal static class StoredFileMapper
{
    public static UploadFileResponse ToDto(this StoredFile storedFile)
    {
        return new UploadFileResponse(storedFile.Id);
    }
}