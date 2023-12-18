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
        _tenantProvider.DisposeDataContext();
    }

    public async Task<bool> DeleteObjectAsync(string key)
    {
        try
        {
            var listVersionsRequest = new ListVersionsRequest
            {
                BucketName = _options.BucketName,
                Prefix = key
            };
            var listVersionsReplicationRequest = new ListVersionsRequest
            {
                BucketName = _options.BucketNameReplication,
                Prefix = key
            };

            /// Delete 
            await _DeleteObjectByBucket(listVersionsRequest, _options.BucketName);

            ///Copy, delete replication
            await _CopyObjectReplyCation(listVersionsReplicationRequest, key);
            await _DeleteObjectByBucket(listVersionsReplicationRequest, _options.BucketNameReplication);
            return true;
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

    public async Task<bool> CopyObjectAsync(string sourceFile, string destinationFile)
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
            var response = await _s3Client.CopyObjectAsync(request);

            return response.HttpStatusCode == HttpStatusCode.OK;
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
            var checkOnlyId = getOnlyId ? request.Key : GetAccessUrl(request.Key);
            return response.HttpStatusCode == HttpStatusCode.OK ? checkOnlyId ?? string.Empty : string.Empty;
        }
        catch (AmazonS3Exception)
        {
            return string.Empty;
        }
    }

    public string GetFolderUploadToPtNum(List<string> folders, long ptNum)
    {
        var tenantId = _tenantProvider.GetDomainName();
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
        var uniqueFileName = $"{fileNameWithoutExtension}-{Guid.NewGuid()}{extension}";
        int fileNameLength = uniqueFileName.Length;
        if (fileNameLength > 100)
        {
            uniqueFileName = uniqueFileName.Substring(fileNameLength - 100, 100);
        }
        return uniqueFileName;
    }

    public string GetFolderUploadOther(List<string> folders)
    {
        var tenantId = _tenantProvider.GetDomainName();
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

    public async Task<(bool valid, string key)> S3FilePathIsExists(string locationFile)
    {
        var listS3Objects = await _s3Client.ListObjectsV2Async(new ListObjectsV2Request
        {
            BucketName = _options.BucketName,
            Prefix = locationFile, // eg myfolder/myimage.jpg (no / at start)
            MaxKeys = 1
        });

        return (listS3Objects.S3Objects.Any(), locationFile);
    }

    #region Private function
    private string GetAccessUrl(string key)
    {
        return $"{_options.BaseAccessUrl}/{key}";
    }
    private static string GetLeftName(string inputString)
    {
        int lastIndex = inputString.LastIndexOf('/');
        int secondLastIndex = inputString.LastIndexOf('/', lastIndex - 1);

        if (lastIndex >= 0 && secondLastIndex >= 0)
        {
            string result = inputString.Substring(0, secondLastIndex + 1);
            return result;
        }
        return string.Empty;
    }
    private static string GetRightName(string inputString)
    {

        int lastIndex = inputString.LastIndexOf('/');
        int secondLastIndex = inputString.LastIndexOf('/', lastIndex - 1);

        if (lastIndex >= 0 && secondLastIndex >= 0)
        {
            string result = inputString.Substring(secondLastIndex + 1, lastIndex - secondLastIndex - 1);
            return result + "/";
        }
        return string.Empty;
    }
    private static string AddFrefixDelete(string inputString)
    {
        char separator = '/';
        string[] segments = inputString.Split(separator);

        for (int i = 0; i < segments.Length; i++)
        {
            if (!string.IsNullOrWhiteSpace(segments[i]))
            {
                segments[i] = $"delete-{segments[i]}";
            }
        }

        return string.Join("/", segments);
    }
    private static string CutString(string substring1, string inputString)
    {
        int firstOccurrenceIndex = inputString.IndexOf(substring1);

        if (firstOccurrenceIndex != -1)
        {
            string part1 = inputString.Substring(0, firstOccurrenceIndex);
            string part2 = inputString.Substring(firstOccurrenceIndex + substring1.Length);
            return part1 + part2;
        }

        return inputString;
    }
    private async Task _DeleteObjectByBucket(ListVersionsRequest listVersionsRequest, string bucketName)
    {
        ListVersionsResponse listVersionsResponse;
        do
        {
            listVersionsResponse = await _s3Client.ListVersionsAsync(listVersionsRequest);

            var objectsToDelete = listVersionsResponse.Versions
                .Select(v => new KeyVersion { Key = v.Key, VersionId = v.VersionId })
                .ToList();

            if (objectsToDelete.Any())
            {
                var deleteObjectsRequest = new DeleteObjectsRequest
                {
                    BucketName = bucketName,
                    Objects = objectsToDelete
                };

                var deleteObjectsResponse = await _s3Client.DeleteObjectsAsync(deleteObjectsRequest);

                // Check the response for any errors
                if (deleteObjectsResponse.DeleteErrors.Any())
                {
                    Console.WriteLine("Some objects could not be deleted. Error details:");
                    foreach (var error in deleteObjectsResponse.DeleteErrors)
                    {
                        Console.WriteLine($"Object Key: {error.Key}, VersionId: {error.VersionId}, Code: {error.Code}, Message: {error.Message}");
                    }
                }
            }

            // Set markers for the next iteration
            listVersionsRequest.KeyMarker = listVersionsResponse.NextKeyMarker;
            listVersionsRequest.VersionIdMarker = listVersionsResponse.NextVersionIdMarker;

        } while (listVersionsResponse.IsTruncated);
        Console.WriteLine("Objects deleted in replication successfully.");

    }
    private async Task _CopyObjectReplyCation(ListVersionsRequest listVersionsReplicationRequest, string key)
    {
        if (!key.EndsWith("/"))
        {
            key += "/";
        }
        ListVersionsResponse listVersionsReplicationResponse;
        do
        {
            listVersionsReplicationResponse = await _s3Client.ListVersionsAsync(listVersionsReplicationRequest);
            /// Copy to Replication (frefix: delete-)
            var listVersionLastest = listVersionsReplicationResponse.Versions.Where(i => i.IsLatest && i.IsDeleteMarker == false);
            if (listVersionLastest.Any())
            {
                var rootFolder = string.Empty;
                int count = key.Split("/").Length - 1;
                if (count == 1)
                {
                    rootFolder = $"delete-{key}";
                }
                else
                {
                    rootFolder = GetLeftName(key) + "delete-" + GetRightName(key);
                }
                Parallel.ForEach(listVersionLastest, version =>
                {
                    try
                    {
                        var copyObjectRequest = new CopyObjectRequest
                        {
                            SourceBucket = _options.BucketNameReplication,
                            SourceKey = version.Key,
                            DestinationBucket = _options.BucketNameReplication,
                        };
                        if (version.Key.EndsWith("/"))
                        {

                            char separator = '/';
                            var checkKey = key;
                            if (!key.EndsWith(separator.ToString()))
                            {
                                checkKey = key + separator;
                            }
                            var cutString = CutString(checkKey, version.Key);
                            copyObjectRequest.DestinationKey = rootFolder + AddFrefixDelete(cutString);
                        }
                        else
                        {
                            char separator = '/';
                            var checkKey = key;
                            if (!key.EndsWith(separator.ToString()))
                            {
                                checkKey = key + separator;
                            }
                            var cutString = CutString(checkKey, version.Key);
                            copyObjectRequest.DestinationKey = rootFolder + AddFrefixDelete(cutString);
                        }
                        Console.WriteLine(copyObjectRequest.DestinationKey);
                        var copyObjectResponse = _s3Client.CopyObjectAsync(copyObjectRequest).Result;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception copy {version.Key}: {ex.Message}");
                    }

                });
            }
            // Set markers for the next iteration
            listVersionsReplicationRequest.KeyMarker = listVersionsReplicationResponse.NextKeyMarker;
            listVersionsReplicationRequest.VersionIdMarker = listVersionsReplicationResponse.NextVersionIdMarker;

        } while (listVersionsReplicationResponse.IsTruncated);
        Console.WriteLine("Objects copied to replication successfully.");
    }
    #endregion
}
