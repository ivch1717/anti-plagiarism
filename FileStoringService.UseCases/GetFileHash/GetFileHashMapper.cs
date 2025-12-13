using FileStoringService.Entities;

namespace FileStoringService.UseCases.GetFileHash;

internal static class GetFileHashMapper
{
    public static GetFileHashResponse ToDto(this StoredFile file)
        => new GetFileHashResponse(file.ContentHash);
}