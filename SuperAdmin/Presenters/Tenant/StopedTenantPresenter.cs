using SuperAdmin.Constants;
using SuperAdmin.Responses;
using SuperAdminAPI.Reponse.Tenant;
using UseCase.SuperAdmin.StopedTenant;
using UseCase.SuperAdmin.TerminateTenant;

namespace SuperAdminAPI.Presenters.Tenant
{
    public class StopedTenantPresenter
    {
        public Response<StopedTenantResponse> Result { get; private set; } = new();
        public void Complete(StopedTenantOutputData output)
        {
            if (output.Status == StopedTenantStatus.Successed)
            {
                Result.Data = new StopedTenantResponse(true);
            }
            else
            {
                Result.Data = new StopedTenantResponse(false);
            }

            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(StopedTenantStatus status) => status switch
        {
            StopedTenantStatus.Successed => ResponseMessage.Success,
            StopedTenantStatus.Failed => ResponseMessage.Fail,
            StopedTenantStatus.InvalidTenantId => ResponseMessage.InvalidTenantId,
            StopedTenantStatus.TenantDoesNotExist => ResponseMessage.TenantDoesNotExist,
            _ => string.Empty
        };
    }
}
