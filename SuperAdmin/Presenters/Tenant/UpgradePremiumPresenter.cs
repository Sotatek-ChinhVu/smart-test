using SuperAdmin.Constants;
using SuperAdmin.Responses;
using SuperAdminAPI.Reponse.Tenant;
using UseCase.SuperAdmin.UpgradePremium;

namespace SuperAdminAPI.Presenters.Tenant
{
    public class UpgradePremiumPresenter
    {
        public Response<UpgradePremiumResponse> Result { get; private set; } = new();
        public void Complete(UpgradePremiumOutputData output)
        {
            if (output.Status == UpgradePremiumStatus.Successed)
            {
                Result.Data = new UpgradePremiumResponse(true);
            }
            else
            {
                Result.Data = new UpgradePremiumResponse(false);
            }

            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(UpgradePremiumStatus status) => status switch
        {
            UpgradePremiumStatus.Successed => ResponseMessage.Success,
            UpgradePremiumStatus.Failed => ResponseMessage.Fail,
            UpgradePremiumStatus.InvalidTenantId => ResponseMessage.InvalidTenantId,
            _ => string.Empty
        };
    }
}
