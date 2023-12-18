using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using AWSSDK.Constants;

namespace AWSSDK.Common
{
    public static class S3Action
    {
        public static async Task CreateFolderAsync(AmazonS3Client sourceS3Client, string bucketName, string folderName)
        {
            try
            {
                if (!folderName.EndsWith("/"))
                {
                    folderName += "/";
                }

                var request = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = folderName,
                    ContentBody = ""
                };

                await sourceS3Client.PutObjectAsync(request);

                Console.WriteLine($"Folder '{folderName}' created successfully in bucket '{bucketName}'.");
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"S3 Error creating folder: '{ex.Message}'");
                throw new Exception($"S3 Error creating folder: '{ex.Message}'");
            }
        }

        /// <summary>
        /// Create Folder backup
        /// </summary>
        /// <param name="s3Client"></param>
        /// <param name="sourceBucket"></param>
        /// <param name="sourceFolder"></param>
        /// <param name="backupBucket"></param>
        /// <param name="backupFolder"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task BackupFolderAsync(AmazonS3Client s3Client, string sourceBucket, string sourceFolder, string backupBucket, string backupFolder)
        {
            try
            {
                if (!sourceFolder.EndsWith("/"))
                {
                    sourceFolder += "/";
                }

                if (!backupFolder.EndsWith("/"))
                {
                    backupFolder += "/";
                }

                // Get list object in sourceFolder
                var listObjectsRequest = new ListObjectsV2Request
                {
                    BucketName = sourceBucket,
                    Prefix = sourceFolder,
                };


                // Coppy sourceFolder to backupFolder
                ListObjectsV2Response listObjectsResponse;
                do
                {
                    // Get list object pagiging
                    listObjectsResponse = await s3Client.ListObjectsV2Async(listObjectsRequest);
                    if (!listObjectsResponse.S3Objects.Any())
                    {
                        Console.WriteLine($"Objects in folder '{sourceFolder}' not found.");
                        return;
                    }
                    
                    foreach (var obj in listObjectsResponse.S3Objects)
                    {
                        var copyObjectRequest = new CopyObjectRequest
                        {
                            SourceBucket = sourceBucket,
                            SourceKey = obj.Key,
                            DestinationBucket = backupBucket,
                            DestinationKey = backupFolder + obj.Key.Substring(sourceFolder.Length),
                        };

                        await s3Client.CopyObjectAsync(copyObjectRequest);
                    }

                    listObjectsRequest.ContinuationToken = listObjectsResponse.NextContinuationToken;
                } while (listObjectsResponse.IsTruncated);
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"S3 Error backing up folder: '{ex.Message}'");
                throw new Exception($"S3 Error backing up folder: '{ex.Message}'");
            }
        }

        /// <summary>
        /// Uploads the specified file.Multiple threads are used to read the file and perform multiple uploads in parallel
        /// </summary>
        /// <param name="s3Client"></param>
        /// <param name="bucketName"></param>
        /// <param name="folderName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<bool> UploadFileWithProgressAsync(AmazonS3Client s3Client, string bucketName, string folderName, string filePath)
        {
            try
            {
                var transferUtility = new TransferUtility(s3Client);
                await transferUtility.UploadAsync(filePath, bucketName, folderName);

                Console.WriteLine($"Successfully uploaded {folderName} to {bucketName}.");
                return true;
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"S3 Error uploading file: '{ex.Message}'");
                throw new Exception($"S3 Error uploading file: '{ex.Message}'");
            }
        }

        public static async Task DeleteObjectsInFolderAsync(AmazonS3Client sourceS3Client, string bucketName, string folderKey)
        {
            try
            {
                ListObjectsV2Request request = new ListObjectsV2Request
                {
                    BucketName = bucketName,
                    Prefix = folderKey
                };

                ListObjectsV2Response response;
                do
                {
                    response = await sourceS3Client.ListObjectsV2Async(request);

                    Parallel.ForEach(response.S3Objects, obj =>
                    {
                        var deleteObjectRequest = new DeleteObjectRequest
                        {
                            BucketName = bucketName,
                            Key = obj.Key
                        };
                        var sourceTransterUtility = new TransferUtility(sourceS3Client);
                        sourceTransterUtility.S3Client.DeleteObjectAsync(deleteObjectRequest).Wait();
                    });

                    request.ContinuationToken = response.NextContinuationToken;
                } while (response.IsTruncated);

                Console.WriteLine($"Objects in folder '{folderKey}' deleted successfully.");
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"S3 Error deleting objects in folder: '{ex.Message}'");
                throw new Exception($"S3 Error deleting objects in folder: '{ex.Message}'");
            }
        }

        static async Task DeleteAllVersionObject(string folderKey, AmazonS3Client s3Client, string bucketName)
        {
            var listVersionsRequest = new ListVersionsRequest
            {
                BucketName = bucketName,
                Prefix = folderKey
            };
            ListVersionsResponse listVersionsResponse;
            do
            {
                listVersionsResponse = await s3Client.ListVersionsAsync(listVersionsRequest);

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

                    var deleteObjectsResponse = await s3Client.DeleteObjectsAsync(deleteObjectsRequest);

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
        }

        public static async Task CopyObjectsInFolderAsync(AmazonS3Client sourceClient, string sourceBucketName, string folderKey, AmazonS3Client destinationClient, string destinationBucketName, bool prefixDelete)
        {
            try
            {
                if (!folderKey.EndsWith("/"))
                {
                    folderKey += "/";
                }
                var folder = folderKey;
                ListObjectsV2Request request = new ListObjectsV2Request
                {
                    BucketName = sourceBucketName,
                    Prefix = folderKey
                };

                ListObjectsV2Response response;
                do
                {
                    response = await sourceClient.ListObjectsV2Async(request);
                    if (!response.S3Objects.Any())
                    {
                        if (!prefixDelete)
                        {
                            Console.WriteLine($"Objects in folder '{folder}' not found.");
                            return;
                        }
                        
                        int count = folderKey.Split("/").Length - 1;
                        if (count == 1)
                        {
                            folderKey = $"delete-{folderKey}";
                        }
                        else
                        {
                            folderKey = CommonConstants.GetLeftName(folderKey) + "delete-" + CommonConstants.GetRightName(folderKey);
                        }
                        request.Prefix = folderKey;
                        response = await sourceClient.ListObjectsV2Async(request);
                        if (!response.S3Objects.Any())
                        {
                            Console.WriteLine($"Objects in folder '{folder}' not found.");
                            return;
                        }
                    }
                    Parallel.ForEach(response.S3Objects, async obj =>
                    {
                        var destinationKey = folderKey + obj.Key.Substring(folderKey.Length);

                        var copyObjectRequest = new CopyObjectRequest
                        {

                            SourceBucket = sourceBucketName,
                            SourceKey = obj.Key,
                            DestinationBucket = destinationBucketName,

                        };
                        if (prefixDelete)
                        {
                            destinationKey = CommonConstants.RemoveDeleteString(destinationKey);
                            copyObjectRequest.DestinationKey = destinationKey;
                            Console.WriteLine(destinationKey);
                            var destinationTransterUtility = new TransferUtility(destinationClient);
                            destinationTransterUtility.S3Client.CopyObjectAsync(copyObjectRequest).Wait();
                        }
                        else
                        {
                            if (!CommonConstants.CheckCondition(destinationKey))
                            {
                                copyObjectRequest.DestinationKey = destinationKey;
                                Console.WriteLine(destinationKey);
                                var destinationTransterUtility = new TransferUtility(destinationClient);
                                destinationTransterUtility.S3Client.CopyObjectAsync(copyObjectRequest).Wait();
                            }
                        }
                        await DeleteAllVersionObject(folderKey,sourceClient,sourceBucketName);
                    });

                    request.ContinuationToken = response.NextContinuationToken;
                } while (response.IsTruncated);

                Console.WriteLine($"Restore objects in folder '{folderKey}' successfully.");
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error restore objects in folder '{folderKey}': '{ex.Message}'");
                throw new Exception($"Error restore objects in folder '{folderKey}': '{ex.Message}'");
            }
        }
    }
}
