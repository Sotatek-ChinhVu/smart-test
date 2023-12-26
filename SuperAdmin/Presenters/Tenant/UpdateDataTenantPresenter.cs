using SuperAdmin.Constants;
using SuperAdmin.Responses;
using SuperAdminAPI.Reponse.Tenant;
using UseCase.SuperAdmin.UpdateDataTenant;

namespace SuperAdminAPI.Presenters.Tenant
{
    public class UpdateDataTenantPresenter
    {
        public Response<UpdateDataTenantResponse> Result { get; private set; } = new();

        public void Complete(UpdateDataTenantOutputData outputData)
        {
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(UpdateDataTenantStatus status) => status switch
        {
            UpdateDataTenantStatus.Successed => ResponseMessage.Success,
            UpdateDataTenantStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
