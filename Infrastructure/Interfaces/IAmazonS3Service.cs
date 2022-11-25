namespace Infrastructure.Interfaces;

public interface IAmazonS3Service
{
    Task<string> UploadAnObjectAsync(bool addToTenant, string subFolder, string fileName, Stream stream);
    Task<string> UploadAnObjectAsync(bool addToTenant, string subFolder, string fileName, MemoryStream memoryStream);
    Task<string> UploadObjectForTenantAsync(string rootFolder, string ptId, string subFolder, string fileName, Stream stream);
    Task<string> UploadObjectForTenantAsync(string rootFolder, string ptId, string subFolder, string fileName, MemoryStream memoryStream);
    Task<bool> ObjectExistsAsync(string key);
    Task<bool> DeleteObjectAsync(string key);
    Task<List<string>> GetListObjectAsync(string prefix);
}
