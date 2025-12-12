using System.IO;
using System.Security.Cryptography;
using FileStoringService.Entities;

namespace FileStoringService.UseCases.UploadFile;

internal sealed  class UploadFileRequestHandler : IUploadFileRequestHandler
{
    private readonly IUploadFileRepository _repository;
    private readonly IFileStorage _fileStorage;
    
    public UploadFileRequestHandler(
        IUploadFileRepository repository,
        IFileStorage fileStorage)
    {
        _repository = repository;
        _fileStorage = fileStorage;
    }
    
    public UploadFileResponse Handle(UploadFileRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FileName))
        {
            throw new ArgumentException("Имя файла не может быть пустым");
        }

        if (request.SizeInBytes < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request.SizeInBytes), "Размер файла не может быть отрицательным");
        }

        if (request.Content == null)
        {
            throw new ArgumentNullException(nameof(request.Content));
        }
        
        var contentHash = ComputeContentHash(request.Content);
        
        if (request.Content.CanSeek)
        {
            request.Content.Position = 0;
        }
        
        var id = Guid.NewGuid();
        var storageKey = BuildStorageKey(id, request.FileName);
        
        _fileStorage.Save(storageKey, request.Content);
        
        var storedFile = new StoredFile(
            id: id,
            storageKey: storageKey,
            fileName: request.FileName,
            contentType: request.ContentType ?? "application/octet-stream",
            sizeInBytes: request.SizeInBytes,
            contentHash: contentHash
        );
        
        _repository.Add(storedFile);
        return storedFile.ToDto();
    }
    
    private static string ComputeContentHash(Stream content)
    {
        using var sha256 = SHA256.Create();
        
        var hashBytes = sha256.ComputeHash(content);
        return Convert.ToHexString(hashBytes);
    }
    
    private static string BuildStorageKey(Guid id, string fileName) {
        var safeFileName = fileName.Replace(" ", "_"); 
        return $"files/{DateTime.UtcNow:yyyy/MM}/{id}_{safeFileName}"; 
    }
}