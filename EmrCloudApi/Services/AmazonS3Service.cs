using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using EmrCloudApi.Common;
using EmrCloudApi.Configs.Options;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text;

namespace EmrCloudApi.Services;

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

    public async Task<string> UploadAnObjectAsync(string fileName, Stream stream)
    {
        try
        {
            var request = new PutObjectRequest
            {
                BucketName = _options.BucketName,
                Key = GetUniqueKey(fileName),
                InputStream = await stream.ToMemoryStreamAsync(),
            };

            var response = await _s3Client.PutObjectAsync(request);
            return response.HttpStatusCode == HttpStatusCode.OK ? GetAccessUrl(request.Key) : string.Empty;
        }
        catch (AmazonS3Exception)
        {
            return string.Empty;
        }
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

    private string GetUniqueKey(string fileName)
    {
        var tenantId = _tenantProvider.GetTenantId();
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        var extension = Path.GetExtension(fileName);
        return $"{tenantId}/{fileNameWithoutExtension}-{Guid.NewGuid()}{extension}";
    }

    private string GetAccessUrl(string key)
    {
        return $"{_options.BaseAccessUrl}/{key}";
    }

    public async Task<List<string>> GetListObjectAsync(string key)
    {
        List<string> listObjects = new();

        var listRequest = new ListObjectsV2Request
        {
            BucketName = _options.BucketName,
        };

        ListObjectsV2Response listResponse;
        do
        {
            // Get a list of objects
            listResponse = await _s3Client.ListObjectsV2Async(listRequest);
            foreach (S3Object obj in listResponse.S3Objects)
            {
                var objectResponse = await _s3Client.GetObjectAsync(new GetObjectRequest { 
                    BucketName = obj.BucketName,
                    Key = obj.Key,
                });
                var bytes = new byte[objectResponse.ResponseStream.Length];
                objectResponse.ResponseStream.Read(bytes, 0, bytes.Length);
                var response = Encoding.UTF8.GetString(bytes);
            }
            listRequest.ContinuationToken = listResponse.NextContinuationToken;
        } while (listResponse.IsTruncated);

        return listObjects;
    }
}
