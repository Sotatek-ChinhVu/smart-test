﻿using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using AWSSDK.Constants;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

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

                var listObjectsResponse = await s3Client.ListObjectsV2Async(listObjectsRequest);

                // Coppy sourceFolder to backupFolder
                foreach (var s3Object in listObjectsResponse.S3Objects)
                {
                    var copyObjectRequest = new CopyObjectRequest
                    {
                        SourceBucket = sourceBucket,
                        SourceKey = s3Object.Key,
                        DestinationBucket = backupBucket,
                        DestinationKey = backupFolder + s3Object.Key.Substring(sourceFolder.Length),
                    };

                    await s3Client.CopyObjectAsync(copyObjectRequest);

                    Console.WriteLine($"Object '{s3Object.Key}' backed up successfully to '{backupFolder}' in bucket '{backupBucket}'.");
                }
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"S3 Error backing up folder: '{ex.Message}'");
                throw new Exception($"S3 Error backing up folder: '{ex.Message}'");
            }
        }

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
