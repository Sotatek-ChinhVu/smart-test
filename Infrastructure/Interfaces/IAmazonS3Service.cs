namespace Infrastructure.Interfaces;

public interface IAmazonS3Service
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="fileName"></param>
    /// <param name="stream"></param>
    /// <param name="getOnlyId">if true. only return Id file on AWS</param>
    /// <returns></returns>
    Task<string> UploadObjectAsync(string path, string fileName, Stream stream, bool getOnlyId = false);

    Task<string> UploadObjectAsync(string path, string fileName, MemoryStream memoryStream, bool getOnlyId = false);

    Task<bool> ObjectExistsAsync(string key);

    Task<bool> DeleteObjectAsync(string key);

    Task<bool> DeleteLastestVerObjectAsync(string key);

    Task<bool> MoveObjectAsync(string sourceFile, string destinationFile);

    Task<bool> CopyObjectAsync(string sourceFile, string destinationFile);

    Task<List<string>> GetListObjectAsync(string prefix);

    string GetFolderUploadToPtNum(List<string> folders, long ptNum);

    string GetFolderUploadOther(List<string> folders);

    string GetUniqueFileNameKey(string fileName);

    string GetAccessBaseS3();

    Task<(bool valid, string key)> S3FilePathIsExists(string locationFile);

    void Dispose();
}
