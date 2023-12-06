
using AWSSDK.Constants;
using AWSSDK.Interfaces;
using UseCase.SuperAdmin.RestoreObjectS3Tenant;

namespace Interactor.SuperAdmin
{
    public class RestoreObjectS3TenantInteractor : IRestoreObjectS3TenantInputPort
    {
        private readonly IAwsSdkService _awsSdkService;
        public RestoreObjectS3TenantInteractor(IAwsSdkService awsSdkService)
        {
            _awsSdkService = awsSdkService;
        }
        public RestoreObjectS3TenantOutputData Handle(RestoreObjectS3TenantInputData inputData)
        {
            try
            {
                if (string.IsNullOrEmpty(inputData.ObjectName))
                {
                    return new RestoreObjectS3TenantOutputData(RestoreObjectS3TenantStatus.Failed);
                }
                var restoreObjectS3 = _awsSdkService.CopyObjectsInFolderAsync(
                    ConfigConstant.SourceBucketName,
                    inputData.ObjectName,
                    ConfigConstant.DestinationBucketName,
                    inputData.ObjectName);
                restoreObjectS3.Wait();
                return new RestoreObjectS3TenantOutputData(RestoreObjectS3TenantStatus.Success);
            }
            catch
            {
                return new RestoreObjectS3TenantOutputData(RestoreObjectS3TenantStatus.Failed);
            }
            finally
            {
            }
        }
    }
}
