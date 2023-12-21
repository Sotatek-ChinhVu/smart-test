﻿using Amazon.S3;
using Amazon.S3.Model;
using Domain.Models.ReleasenoteRead;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositoriesp;

public class ReleasenoteReadRepository : RepositoryBase, IReleasenoteReadRepository
{
    public ReleasenoteReadRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }
    public List<string> GetListReleasenote(int hpId, int userId)
    {
        List<ReleasenoteRead> releasenote = NoTrackingDataContext.ReleasenoteReads.Where(u => u.HpId == hpId && u.UserId == userId).ToList();
        if (releasenote == null)
        {
            return new List<string>();
        }

        var result = new List<string>();

        foreach (var version in releasenote.Select(u => u.Version))
        {
            result.Add(version.Substring(0, 2) + "." + version.Substring(2, 2) + "." + version.Substring(4, 2) + "." + version.Substring(6, 2) + "." + version.Substring(8, 2));
        }

        return result;
    }

    public async Task<List<ReleasenoteReadModel>> GetLoadListVersion(int hpId, int userId, AmazonS3Client sourceClient, string sourceBucketName)
    {
        var listHeader = GetListReleasenote(hpId, userId);
        ListObjectsV2Request request = new ListObjectsV2Request
        {
            BucketName = "develop-smartkarte-images-bucket",
            Prefix = "data/reference"
        };
        ListObjectsV2Response response = await sourceClient.ListObjectsV2Async(request);
        List<Task<string>> fileUrlTasks = new List<Task<string>>();

        foreach (S3Object s3Object in response.S3Objects)
        {
            if (!s3Object.Key.EndsWith("/"))
            {
                fileUrlTasks.Add(GetObjectUrlAsync(sourceClient, "develop-smartkarte-images-bucket", s3Object.Key));
            }
        }

        string[] fileUrls = await Task.WhenAll(fileUrlTasks);

        List<ReleasenoteReadModel> result = new List<ReleasenoteReadModel>();
        foreach (var item in listHeader)
        {
            string path = string.Empty;
            List<string> subfiles = new();
            for (int i = 0; i < fileUrls.Length; i++)
            {
                if (fileUrls[i].Contains(item))
                {
                    Uri uri = new Uri(fileUrls[i]);
                    if (!fileUrls[i].Contains("subfiles"))
                    {
                        path = fileUrls[i];
                    }

                    if (fileUrls[i].Contains("subfiles"))
                    {
                        subfiles.Add(fileUrls[i]);
                    }
                    //string fileName = Path.GetFileName(uri.LocalPath);
                    //string folderName = Path.GetDirectoryName(uri.LocalPath) ?? "";

                    /*if (folderName.LastIndexOf("\\") != -1)
                    {
                        folderName = folderName.Substring(folderName.LastIndexOf("\\") + 1);
                    }*/

                    //Dictionary<string, string> fileNamePath = new Dictionary<string, string>();
                    //fileNamePath.Add(fileName, fileUrls[i]);
                }
            }

            result.Add(new ReleasenoteReadModel(item, subfiles, path));
        }

        return result;
    }

    // Phương thức bất đồng bộ để lấy URL của file
    private async Task<string> GetObjectUrlAsync(AmazonS3Client client, string bucketName, string key)
    {
        return await Task.Run(() => GetObjectUrl(client, bucketName, key));
    }

    private static string GetObjectUrl(AmazonS3Client sourceClient, string bucketName, string objectKey)
    {
        GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = objectKey,
            Expires = DateTime.UtcNow.AddHours(1) // Thời gian hết hạn URL, bạn có thể điều chỉnh theo nhu cầu của mình
        };

        string url = sourceClient.GetPreSignedURL(request);

        return url;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}