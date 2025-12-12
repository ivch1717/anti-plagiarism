namespace FileStoringService.UseCases.UploadFile;

public interface IFileStorageUploadFile
{
    void Save(string storageKey, Stream content);
}