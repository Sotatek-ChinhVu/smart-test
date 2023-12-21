using Amazon.S3;
using AWSSDK.Constants;
using Domain.Models.ReleasenoteRead;
using Helper.Constants;
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
                var accessKey = LoadListVersionEnum.AccessKey;
                var secretKey = LoadListVersionEnum.SecretKey;
                var config = new AmazonS3Config
                {
                    RegionEndpoint = ConfigConstant.RegionSource
                };

                using (var s3Client = new AmazonS3Client(accessKey, secretKey, config))
                {
                    var result = _releasenoteReadRepository.GetLoadListVersion(inputData.HpId, inputData.UserId, s3Client);
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
