using Amazon.S3;
using AWSSDK.Common;
using AWSSDK.Constants;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using UseCase.SuperAdmin.DeleteJunkFileS3;

namespace Interactor.SuperAdmin
{
    public class DeleteJunkFileS3Interactor : IDeleteJunkFileS3InputPort
    {
        private readonly AmazonS3Options _options;
        public DeleteJunkFileS3Interactor(IOptions<AmazonS3Options> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }
        public DeleteJunkFileS3OutputData Handle(DeleteJunkFileS3InputData inputData)
        {
            try
            {
                var accessKey = "AKIAXSCVMXDLRLYZGZ6Q";
                var secretKey = "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";
                var config = new AmazonS3Config
                {
                    RegionEndpoint = ConfigConstant.RegionSource
                };
                using (var s3Client = new AmazonS3Client(accessKey, secretKey, config))
                {
                    S3Action.DeleteJunkFile(s3Client, ConfigConstant.SourceBucketName).Wait();
                }
                return new DeleteJunkFileS3OutputData(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Schedule delete junk file s3: " + ex);
                return new DeleteJunkFileS3OutputData(false);
            }
        }
    }
}
