using FileStoringService.Entities;
using FileStoringService.Infrastructure.Data.Dtos;

namespace FileStoringService.Infrastructure.Data;

internal static class DataMapper
{
    public static StoredFileDto ToDto(this StoredFile entity)
    {
        return new StoredFileDto
        {
            Id = entity.Id,
            StorageKey = entity.StorageKey,
            FileName = entity.FileName,
            ContentType = entity.ContentType,
            SizeInBytes = entity.SizeInBytes,
            ContentHash = entity.ContentHash
        };
    }

    public static StoredFile ToEntity(this StoredFileDto dto)
    {
        return new StoredFile(
            id: dto.Id,
            storageKey: dto.StorageKey,
            fileName: dto.FileName,
            contentType: dto.ContentType,
            sizeInBytes: dto.SizeInBytes,
            contentHash: dto.ContentHash
        );
    }
}