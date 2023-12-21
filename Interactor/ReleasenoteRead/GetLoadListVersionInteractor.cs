using Amazon.S3;
using AWSSDK.Common;
using AWSSDK.Constants;
using Domain.Models.ReleasenoteRead;
using Microsoft.AspNetCore.Mvc;
using UseCase.Releasenote.LoadListVersion;

namespace Interactor.ReleasenoteRead
{
    public class GetLoadListVersionInteractor : IGetLoadListVersionInputPort
    {
        private readonly IReleasenoteReadRepository _releasenoteReadRepository;

        public GetLoadListVersionInteractor(IReleasenoteReadRepository releasenoteReadRepository)
        {
            _releasenoteReadRepository = releasenoteReadRepository;
        }

        public GetLoadListVersionOutputData Handle(GetLoadListVersionInputData inputData)
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
                    //_releasenoteReadRepository.DeleteJunkFile(s3Client, ConfigConstant.SourceBucketName).Wait();
                    var result = _releasenoteReadRepository.GetLoadListVersion(inputData.HpId, inputData.UserId, s3Client, ConfigConstant.SourceBucketName);
                    return new GetLoadListVersionOutputData(result, GetLoadListVersionStatus.Successed);

                }
            }
            finally
            {
                _releasenoteReadRepository.ReleaseResource();
            }
        }
    }
}
