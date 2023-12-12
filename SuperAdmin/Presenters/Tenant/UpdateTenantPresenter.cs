using SuperAdmin.Constants;
using SuperAdmin.Responses;
using SuperAdminAPI.Reponse.Tenant;
using UseCase.SuperAdmin.UpgradePremium;

namespace SuperAdminAPI.Presenters.Tenant
{
    public class UpdateTenantPresenter
    {
        public Response<UpdateTenantResponse> Result { get; private set; } = new();
        public void Complete(UpdateTenantOutputData output)
        {
            if (output.Status == UpdateTenantStatus.Successed)
            {
                Result.Data = new UpdateTenantResponse(true);
            }
            else
            {
                Result.Data = new UpdateTenantResponse(false);
            }

            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(UpdateTenantStatus status) => status switch
        {
            UpdateTenantStatus.Successed => ResponseMessage.Success,
            UpdateTenantStatus.Failed => ResponseMessage.Fail,
            UpdateTenantStatus.InvalidTenantId => ResponseMessage.InvalidTenantId,
            UpdateTenantStatus.InvalidSize => ResponseMessage.InvalidSize,
            UpdateTenantStatus.InvalidSizeType => ResponseMessage.InvalidSizeType,
            UpdateTenantStatus.InvalidDomain => ResponseMessage.InvalidDomain,
            UpdateTenantStatus.NewDomainAleadyExist => ResponseMessage.NewDomainAleadyExist,
            UpdateTenantStatus.RdsDoesNotExist => ResponseMessage.TenantDbDoesNotExistInRDS,
            UpdateTenantStatus.TenantDoesNotExist => ResponseMessage.TenantDoesNotExist,
            UpdateTenantStatus.FailedTenantIsPremium => ResponseMessage.FailedTenantIsPremium,
            _ => string.Empty
        };
    }
}
