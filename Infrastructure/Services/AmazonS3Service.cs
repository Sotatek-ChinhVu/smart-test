﻿using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Infrastructure.Common;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Net;

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

    public async Task<string> UploadAnObjectAsync(string subFolder, string fileName, Stream stream)
    {
        try
        {
            var request = new PutObjectRequest
            {
                BucketName = _options.BucketName,
                Key = GetUniqueKey(subFolder, fileName),
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

    private string GetUniqueKey(string subFolder, string fileName)
    {
        var tenantId = _tenantProvider.GetTenantId();
        var prefix = "tenants/" + tenantId;
        if (!string.IsNullOrEmpty(subFolder))
        {
            prefix += ("/" + subFolder);
        }

        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        var extension = Path.GetExtension(fileName);
        return $"{prefix}/{fileNameWithoutExtension}-{Guid.NewGuid()}{extension}";
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
}
