using FileStoringService.UseCases.UploadFile;
using FileStoringService.UseCases.DownloadFile;
using Microsoft.Extensions.Configuration;

namespace FileStoringService.Infrastructure.Storage;

internal sealed class LocalFileStorage :
    IFileStorageUploadFile,
    IFileStoragelDownloadFile
{
    private readonly string _rootPath;

    public LocalFileStorage(IConfiguration configuration)
    {
        _rootPath = configuration["FileStorage:RootPath"] ?? "storage";
        Directory.CreateDirectory(_rootPath);
    }

    public void Save(string storageKey, Stream content)
    {
        var fullPath = GetFullPath(storageKey);

        var dir = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrWhiteSpace(dir))
            Directory.CreateDirectory(dir);

        using var fileStream = File.Create(fullPath);
        content.CopyTo(fileStream);
    }

    public Stream OpenRead(string storageKey)
    {
        var fullPath = GetFullPath(storageKey);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException("Файл не найден", fullPath);

        return File.OpenRead(fullPath);
    }

    private string GetFullPath(string storageKey)
    {
        var safeKey = storageKey.Replace('/', Path.DirectorySeparatorChar);
        return Path.Combine(_rootPath, safeKey);
    }
}