using SuperAdmin.Constants;
using SuperAdmin.Responses;
using SuperAdminAPI.Reponse.Tenant;
using UseCase.SuperAdmin.RestoreObjectS3Tenant;
using UseCase.SuperAdmin.UpdateTenant;

namespace SuperAdminAPI.Presenters.Tenant
{
    public class UpdateTenantPresenter
    {
        public Response<UpdateTenantResponse> Result { get; private set; } = new();

        public void Complete(UpdateTenantOutputData outputData)
        {
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(UpdateTenantStatus status) => status switch
        {
            UpdateTenantStatus.Successed => ResponseMessage.Success,
            UpdateTenantStatus.Failed => ResponseMessage.Fail,
            _ => string.Empty
        };
    }
}
