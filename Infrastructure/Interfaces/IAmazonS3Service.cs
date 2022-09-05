namespace Infrastructure.Interfaces;

public interface IAmazonS3Service
{
    Task<string> UploadAnObjectAsync(string fileName, Stream stream);
    Task<bool> ObjectExistsAsync(string key);
}
