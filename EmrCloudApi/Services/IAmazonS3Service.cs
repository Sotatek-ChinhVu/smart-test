namespace EmrCloudApi.Services;

public interface IAmazonS3Service
{
    Task<string> UploadAnObjectAsync(string fileName, Stream stream);
}
