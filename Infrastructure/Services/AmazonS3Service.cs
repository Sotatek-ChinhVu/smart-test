using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Infrastructure.Common;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text;

namespace Infrastructure.Services;

public sealed class AmazonS3Service : IAmazonS3Service, IDisposable
{
    private readonly AmazonS3Options _options;
    private readonly IAmazonS3 _s3Client;
    private readonly ITenantProvider _tenantProvider;

    public AmazonS3Service(IOptions<AmazonS3Options> optionsAccessor, ITenantProvider tenantProvider)
    {
        _options = optionsAccessor.Value;
        var regionEndpoint = RegionEndpoint.GetBySystemName(_options.Region);
        _s3Client = new AmazonS3Client(_options.AwsAccessKeyId, _options.AwsSecretAccessKey, regionEndpoint);
        _tenantProvider = tenantProvider;
    }

    public async Task<bool> ObjectExistsAsync(string key)
    {
        try
        {
            var response = await _s3Client.GetObjectAsync(_options.BucketName, key);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }
        catch (AmazonS3Exception e)
        {
            if (e.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }

            throw;
        }
    }

    public void Dispose()
    {
        _s3Client.Dispose();
    }

    private string GetAccessUrl(string key)
    {
        return $"{_options.BaseAccessUrl}/{key}";
    }

    public async Task<bool> DeleteObjectAsync(string key)
    {
        try
        {
            var response = await _s3Client.DeleteObjectAsync(_options.BucketName, key);
            return Convert.ToBoolean(response.DeleteMarker);
        }
        catch (AmazonS3Exception)
        {
            return false;
        }
    }

    public async Task<bool> MoveObjectAsync(string sourceFile, string destinationFile)
    {
        try
        {
            var request = new CopyObjectRequest
            {
                SourceBucket = _options.BucketName,
                SourceKey = sourceFile,
                DestinationBucket = _options.BucketName,
                DestinationKey = destinationFile
            };
            await _s3Client.CopyObjectAsync(request);

            var response = await _s3Client.DeleteObjectAsync(_options.BucketName, sourceFile);
            return Convert.ToBoolean(response.DeleteMarker);
        }
        catch (AmazonS3Exception)
        {
            return false;
        }
    }

    public async Task<List<string>> GetListObjectAsync(string prefix)
    {
        List<string> listObjects = new();

        var listRequest = new ListObjectsV2Request
        {
            BucketName = _options.BucketName,
            Prefix = prefix
        };

        ListObjectsV2Response listResponse;
        do
        {
            // Get a list of objects
            listResponse = await _s3Client.ListObjectsV2Async(listRequest);
            foreach (S3Object obj in listResponse.S3Objects)
            {
                listObjects.Add(obj.Key);
            }
            listRequest.ContinuationToken = listResponse.NextContinuationToken;
        } while (listResponse.IsTruncated);

        return listObjects;
    }

    public async Task<string> UploadObjectAsync(string path, string fileName, Stream stream, bool getOnlyId = false)
    {
        var memoryStream = await stream.ToMemoryStreamAsync();
        return await UploadObjectAsync(path, fileName, memoryStream, getOnlyId);
    }

    public async Task<string> UploadObjectAsync(string path, string fileName, MemoryStream memoryStream, bool getOnlyId = false)
    {
        try
        {
            var request = new PutObjectRequest
            {
                BucketName = _options.BucketName,
                Key = path + fileName,
                InputStream = memoryStream,
            };
            var response = await _s3Client.PutObjectAsync(request);
            return response.HttpStatusCode == HttpStatusCode.OK ? getOnlyId ? request.Key : GetAccessUrl(request.Key) : string.Empty;
        }
        catch (AmazonS3Exception)
        {
            return string.Empty;
        }
    }

    public string GetFolderUploadToPtNum(List<string> folders, long ptNum)
    {
        var tenantId = _tenantProvider.GetClinicID();
        var ptNumString = ptNum.ToString();
        if (ptNum.ToString().Length < 4)
        {
            ptNumString = ptNumString.PadLeft(4, '0');
        }
        string last4Characters = ptNumString.Substring(ptNumString.Length - 4);
        StringBuilder result = new();
        result.Append(tenantId);
        result.Append("/");
        foreach (var item in folders)
        {
            result.Append(item);
            result.Append("/");
        }
        result.Append(last4Characters.Substring(0, 2));
        result.Append("/");
        result.Append(last4Characters.Substring(2, 2));
        result.Append("/");
        result.Append(ptNum.ToString());
        result.Append("/");
        return result.ToString();
    }

    public string GetUniqueFileNameKey(string fileName)
    {
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        var extension = Path.GetExtension(fileName);
        return $"{fileNameWithoutExtension}-{Guid.NewGuid()}{extension}";
    }

    public string GetFolderUploadOther(List<string> folders)
    {
        var tenantId = _tenantProvider.GetClinicID();
        StringBuilder result = new();
        result.Append(tenantId);
        result.Append("/");
        foreach (var item in folders)
        {
            result.Append(item);
            result.Append("/");
        }
        return result.ToString();
    }

    public string GetAccessBaseS3() => $"{_options.BaseAccessUrl}/";
}
