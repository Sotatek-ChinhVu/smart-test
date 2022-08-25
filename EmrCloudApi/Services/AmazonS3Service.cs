using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using EmrCloudApi.Common;
using EmrCloudApi.Configs.Options;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;

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

    public async Task<string?> UploadAnObjectAsync(string fileName, Stream stream)
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
            return response.HttpStatusCode == HttpStatusCode.OK ? GetAccessUrl(request.Key) : null;
        }
        catch (AmazonS3Exception)
        {
            return null;
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
}
