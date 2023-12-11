using SuperAdmin.Constants;
using SuperAdmin.Responses;
using SuperAdminAPI.Reponse.Tenant;
using UseCase.SuperAdmin.StopedTenant;
using UseCase.SuperAdmin.TerminateTenant;

namespace SuperAdminAPI.Presenters.Tenant
{
    public class ToggleTenantPresenter
    {
        public Response<ToggleTenantResponse> Result { get; private set; } = new();
        public void Complete(ToggleTenantOutputData output)
        {
            if (output.Status == ToggleTenantStatus.Successed)
            {
                Result.Data = new ToggleTenantResponse(true);
            }
            else
            {
                Result.Data = new ToggleTenantResponse(false);
            }

            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(ToggleTenantStatus status) => status switch
        {
            ToggleTenantStatus.Successed => ResponseMessage.Success,
            ToggleTenantStatus.Failed => ResponseMessage.Fail,
            ToggleTenantStatus.InvalidTenantId => ResponseMessage.InvalidTenantId,
            ToggleTenantStatus.TenantDoesNotExist => ResponseMessage.TenantDoesNotExist,
            _ => string.Empty
        };
    }
}
