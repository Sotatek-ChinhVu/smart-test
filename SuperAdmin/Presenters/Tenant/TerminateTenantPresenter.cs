using SuperAdmin.Constants;
using SuperAdmin.Responses;
using SuperAdminAPI.Reponse.Tenant;
using UseCase.SuperAdmin.TerminateTenant;

namespace SuperAdminAPI.Presenters.Tenant
{
    public class TerminateTenantPresenter
    {
        public Response<TerminateTenantResponse> Result { get; private set; } = new();
        public void Complete(TerminateTenantOutputData output)
        {
            if (output.Status == TerminateTenantStatus.Successed)
            {
                Result.Data = new TerminateTenantResponse(true);
            }
            else
            {
                Result.Data = new TerminateTenantResponse(false);
            }

            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(TerminateTenantStatus status) => status switch
        {
            TerminateTenantStatus.Successed => ResponseMessage.Success,
            TerminateTenantStatus.Failed => ResponseMessage.Failed,
            TerminateTenantStatus.InvalidTenantId => ResponseMessage.InvalidTenantId,
            TerminateTenantStatus.TenantDoesNotExist => ResponseMessage.TenantDoesNotExist,
            TerminateTenantStatus.TenantDbDoesNotExistInRDS => ResponseMessage.TenantDbDoesNotExistInRDS,
            TerminateTenantStatus.TenantIsTerminating => ResponseMessage.TenantIsTerminating,
            _ => string.Empty
        };
    }
}
