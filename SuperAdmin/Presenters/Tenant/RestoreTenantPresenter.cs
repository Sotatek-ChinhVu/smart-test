using SuperAdmin.Constants;
using SuperAdmin.Responses;
using SuperAdminAPI.Reponse.Tenant;
using UseCase.SuperAdmin.RestoreTenant;
using UseCase.SuperAdmin.StopedTenant;

namespace SuperAdminAPI.Presenters.Tenant
{
    public class RestoreTenantPresenter
    {
        public Response<RestoreTenantResponse> Result { get; private set; } = new();
        public void Complete(RestoreTenantOutputData output)
        {
            if (output.Status == RestoreTenantStatus.Successed)
            {
                Result.Data = new RestoreTenantResponse(true);
            }
            else
            {
                Result.Data = new RestoreTenantResponse(false);
            }

            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(RestoreTenantStatus status) => status switch
        {
            RestoreTenantStatus.Successed => ResponseMessage.Success,
            RestoreTenantStatus.Failed => ResponseMessage.Failed,
            RestoreTenantStatus.InvalidTenantId => ResponseMessage.InvalidTenantId,
            RestoreTenantStatus.TenantDoesNotExist => ResponseMessage.TenantDoesNotExist,
            _ => string.Empty
        };
    }
}
