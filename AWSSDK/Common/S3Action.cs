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

                    foreach (var obj in response.S3Objects)
                    {
                        var deleteObjectRequest = new DeleteObjectRequest
                        {
                            BucketName = bucketName,
                            Key = obj.Key
                        };
                        var sourceTransterUtility = new TransferUtility(sourceS3Client);
                        await sourceTransterUtility.S3Client.DeleteObjectAsync(deleteObjectRequest);
                    }

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
        public static async Task CopyObjectsInFolderAsync(AmazonS3Client sourceClient, string sourceBucketName, string sourceFolderKey, AmazonS3Client destinationClient, string destinationBucketName, string destinationFolderKey)
        {
            try
            {
                ListObjectsV2Request request = new ListObjectsV2Request
                {
                    BucketName = sourceBucketName,
                    Prefix = sourceFolderKey
                };

                ListObjectsV2Response response;
                do
                {
                    response = await sourceClient.ListObjectsV2Async(request);
                    if (!response.S3Objects.Any())
                    {
                        Console.WriteLine($"Objects in folder '{sourceFolderKey}' not found.");
                        return;
                    }
                    foreach (var obj in response.S3Objects)
                    {
                        var copyObjectRequest = new CopyObjectRequest
                        {
                            SourceBucket = sourceBucketName,
                            SourceKey = obj.Key,
                            DestinationBucket = destinationBucketName,
                            DestinationKey = destinationFolderKey + obj.Key.Substring(sourceFolderKey.Length)
                        };

                        var destinationTransterUtility = new TransferUtility(destinationClient);
                        await destinationTransterUtility.S3Client.CopyObjectAsync(copyObjectRequest);
                    }

                    request.ContinuationToken = response.NextContinuationToken;
                } while (response.IsTruncated);

                Console.WriteLine($"Objects in folder '{sourceFolderKey}' copied to '{destinationFolderKey}' successfully.");
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error copying objects in folder: '{ex.Message}'");
                throw new Exception($"Error copying objects in folder: '{ex.Message}'");
            }
        }
    }
}
