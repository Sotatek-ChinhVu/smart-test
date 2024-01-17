using Amazon.S3;
using Amazon.S3.Model;
using Domain.Models.ReleasenoteRead;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
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
        List<ReleasenoteRead> releasenote = NoTrackingDataContext.ReleasenoteReads.Where(u => u.HpId == hpId && u.UserId == userId).OrderByDescending(x => x.Version).ToList();
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

    public async Task<bool> CheckShowReleaseNote(int hpId, int userId, AmazonS3Client sourceClient)
    {
        var listHeader = GetListReleasenote(hpId, userId);

        ListObjectsV2Request request = new ListObjectsV2Request
        {
            BucketName = LoadListVersionEnum.BucketName,
            Prefix = LoadListVersionEnum.Prefix
        };

        ListObjectsV2Response response = await sourceClient.ListObjectsV2Async(request);
        List<Task<string>> fileUrlTasks = new List<Task<string>>();

        foreach (S3Object s3Object in response.S3Objects)
        {
            if (!s3Object.Key.EndsWith("/"))
            {
                fileUrlTasks.Add(GetObjectUrlAsync(sourceClient, LoadListVersionEnum.BucketName, s3Object.Key));
            }
        }

        string[] fileUrls = await Task.WhenAll(fileUrlTasks);
        List<ReleasenoteReadModel> result = new List<ReleasenoteReadModel>();
        List<string> headers = new List<string>();
        bool showReleaseNote = true;

        foreach (var item in fileUrls)
        {
            var header = GetFolderPath(item);
            headers.Add(header);
        }

        headers = headers.Distinct().OrderByDescending(x => x).ToList();

        foreach (var item in headers)
        {
            if (!listHeader.Contains(item))
            {
                showReleaseNote = true;
                break;
            }
            else
            {
                showReleaseNote = false;
            }
        }
        return showReleaseNote;
    }

    public async Task<List<ReleasenoteReadModel>> GetLoadListVersion(int hpId, int userId, AmazonS3Client sourceClient)
    {
        var listHeader = GetListReleasenote(hpId, userId);

        ListObjectsV2Request request = new ListObjectsV2Request
        {
            BucketName = LoadListVersionEnum.BucketName,
            Prefix = LoadListVersionEnum.Prefix
        };

        ListObjectsV2Response response = await sourceClient.ListObjectsV2Async(request);
        List<Task<string>> fileUrlTasks = new List<Task<string>>();

        foreach (S3Object s3Object in response.S3Objects)
        {
            if (!s3Object.Key.EndsWith("/"))
            {
                fileUrlTasks.Add(GetObjectUrlAsync(sourceClient, LoadListVersionEnum.BucketName, s3Object.Key));
            }
        }

        string[] fileUrls = await Task.WhenAll(fileUrlTasks);
        List<ReleasenoteReadModel> result = new List<ReleasenoteReadModel>();
        List<string> headers = new List<string>();

        foreach (var item in fileUrls)
        {
            var header = GetFolderPath(item);
            headers.Add(header);
        }

        headers = headers.Distinct().OrderByDescending(x => x).ToList();

        foreach (var item in headers)
        {
            string path = string.Empty;
            Dictionary<string, Dictionary<string, string>> subfiles = new();

            for (int i = 0; i < fileUrls.Length; i++)
            {
                if (fileUrls[i].Contains(item))
                {
                    Uri uri = new Uri(fileUrls[i]);

                    if (!fileUrls[i].Contains("subfiles"))
                    {
                        path = fileUrls[i];
                    }

                    Dictionary<string, string> file = new();

                    if (fileUrls[i].Contains("subfiles"))
                    {
                        file.Add(Path.GetExtension(uri.LocalPath), fileUrls[i]);

                        if (Path.GetExtension(uri.LocalPath) != ".db")
                        {
                            subfiles.Add(Path.GetFileNameWithoutExtension(uri.LocalPath), file);
                        }
                    }
                }
            }

            if (subfiles.Count != 0 || path != string.Empty)
            {
                result.Add(new ReleasenoteReadModel(item, subfiles, path));
            }
        }

        return result;
    }

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
            Expires = DateTime.UtcNow.AddHours(1)
        };

        string url = sourceClient.GetPreSignedURL(request);

        return url;
    }

    private string GetFolderPath(string url)
    {
        Uri uri = new Uri(url);
        string path = uri.AbsolutePath;
        string[] pathSegments = path.Split('/');
        string versionSegment = Array.Find(pathSegments, s => s.Contains(".")) ?? "";

        return versionSegment ?? string.Empty;
    }

    public bool UpdateListReleasenote(int hpId, int userId, List<string> versions)
    {
        foreach (var version in versions)
        {
            if (NoTrackingDataContext.ReleasenoteReads.Where(x => x.HpId == hpId && x.UserId == userId).Any(x => x.Version == version.Replace(".", ""))) continue;
            var newEntity = new ReleasenoteRead()
            {
                HpId = hpId,
                UserId = userId,
                Version = version.Replace(".", ""),
                CreateDate = CIUtil.GetJapanDateTimeNow(),
            };
            TrackingDataContext.ReleasenoteReads.Add(newEntity);
        }

        return TrackingDataContext.SaveChanges() > 0;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
