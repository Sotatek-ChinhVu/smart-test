﻿namespace Infrastructure.Interfaces;

public interface IAmazonS3Service
{
    Task<string> UploadObjectAsync(string path, string fileName, Stream stream);

    Task<string> UploadObjectAsync(string path, string fileName, MemoryStream memoryStream);

    Task<bool> ObjectExistsAsync(string key);

    Task<bool> DeleteObjectAsync(string key);

    Task<bool> CopyObjectAsync(string sourceFolder, string fileName, string destinationFolders);

    Task<List<string>> GetListObjectAsync(string prefix);

    string GetFolderUploadToPtNum(List<string> folders, long ptNum);

    string GetFolderUploadOther(List<string> folders);

    string GetUniqueFileNameKey(string fileName);
}
