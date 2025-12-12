namespace FileStoringService.UseCases.UploadFile;

public interface IFileStorage
{
    void Save(string storageKey, Stream content);
    
    Stream OpenRead(string storageKey);
    
    void Delete(string storageKey);
}