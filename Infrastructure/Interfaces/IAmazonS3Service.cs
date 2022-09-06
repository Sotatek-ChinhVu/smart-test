namespace Infrastructure.Interfaces;

public interface IAmazonS3Service
{
    Task<string> UploadAnObjectAsync(string subFolder, string fileName, Stream stream);
    Task<string> UploadAnObjectAsync(string subFolder, string fileName, MemoryStream memoryStream);
    Task<bool> ObjectExistsAsync(string key);
    Task<bool> DeleteObjectAsync(string key);
    Task<List<string>> GetListObjectAsync(string prefix);
}
