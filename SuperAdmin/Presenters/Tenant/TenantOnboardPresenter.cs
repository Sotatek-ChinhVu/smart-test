using SuperAdmin.Constants;
using SuperAdmin.Responses;
using SuperAdminAPI.Reponse.Tenant;
using UseCase.SuperAdmin.TenantOnboard;

namespace SuperAdminAPI.Presenters.Tenant
{
    public class TenantOnboardPresenter
    {
        public Response<TenantOnboardResponse> Result { get; private set; } = new();
        public void Complete(TenantOnboardOutputData output)
        {
            if (output.Status == TenantOnboardStatus.Successed)
            {
                Result.Data = new TenantOnboardResponse(output.Data, output.Status);
            }
            else
            {
                Result.Data = new TenantOnboardResponse(new(), output.Status);
            }

            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(TenantOnboardStatus status) => status switch
        {
            TenantOnboardStatus.Successed => ResponseMessage.Success,
            TenantOnboardStatus.Failed => ResponseMessage.Failed,
            TenantOnboardStatus.InvalidRequest => ResponseMessage.InvalidRequest,
            TenantOnboardStatus.InvalidSize => ResponseMessage.InvalidSize,
            TenantOnboardStatus.InvalidClusterMode => ResponseMessage.InvalidClusterMode,
            TenantOnboardStatus.InvalidSizeType => ResponseMessage.InvalidSizeType,
            TenantOnboardStatus.SubDomainExists => ResponseMessage.SubDomainExists,
            TenantOnboardStatus.InvalidSubDomain => ResponseMessage.InvalidSubDomain,
            TenantOnboardStatus.HopitalExists => ResponseMessage.HopitalExists,
            _ => string.Empty
        };
    }
}
