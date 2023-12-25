using SuperAdmin.Constants;
using SuperAdmin.Responses;
using SuperAdminAPI.Reponse.Tenant;
using UseCase.SuperAdmin.RestoreObjectS3Tenant;

namespace SuperAdminAPI.Presenters.Tenant
{
    public class RestoreObjectS3TenantPresenter : IRestoreObjectS3TenantOutputPort
    {
        public Response<RestoreObjectS3TenantResponse> Result { get; private set; } = new();

        public void Complete(RestoreObjectS3TenantOutputData outputData)
        {
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(RestoreObjectS3TenantStatus status) => status switch
        {
            RestoreObjectS3TenantStatus.Success => ResponseMessage.Success,
            RestoreObjectS3TenantStatus.Failed => ResponseMessage.Failed,
            RestoreObjectS3TenantStatus.SubdomainDoesNotExist => ResponseMessage.SubDomainDoesNotExist,
            RestoreObjectS3TenantStatus.TenantIsProcessOfRestoreS3 => ResponseMessage.TenantIsProcessOfRestoreS3,
            _ => string.Empty
        };
    }
}
