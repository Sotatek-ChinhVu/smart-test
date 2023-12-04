using Amazon.S3;
using Amazon.S3.Model;
using AWSSDK.Constants;

namespace AWSSDK.Common
{
    public static class S3Action
    {
        public static async Task CreateFolderAsync(string bucketName, string folderName)
        {
            try
            {
                var sourceS3Client = new AmazonS3Client(ConfigConstant.SourceAccessKey, ConfigConstant.SourceSecretKey, ConfigConstant.RegionDestination);
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
        public static async Task DeleteObjectsInFolderAsync(string bucketName, string folderKey)
        {
            try
            {
                var sourceS3Client = new AmazonS3Client(ConfigConstant.SourceAccessKey, ConfigConstant.SourceSecretKey, ConfigConstant.RegionDestination);
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

                        await sourceS3Client.DeleteObjectAsync(deleteObjectRequest);
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
    }
}
