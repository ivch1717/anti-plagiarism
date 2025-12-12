namespace FileStoringService.Infrastructure.Data.Dtos;

internal sealed class StoredFileDto
{
    public Guid Id { get; set; }
    public string StorageKey { get; set; } = default!;
    public string FileName { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public long SizeInBytes { get; set; }
    public string ContentHash { get; set; } = default!;
}