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
            UpdateDataTenantStatus.InvalidTenantId => ResponseMessage.InvalidTenantId,
            UpdateDataTenantStatus.TenantDoesNotExist => ResponseMessage.TenantDoesNotExist,
            UpdateDataTenantStatus.TenantNotReadyToUpdate => ResponseMessage.TenantNotReadyToUpdate,
            UpdateDataTenantStatus.UploadFileIncorrectFormat7z => ResponseMessage.UploadFileIncorrectFormat7z,
            UpdateDataTenantStatus.UnzipFile7zError => ResponseMessage.UnzipFile7zError,
            UpdateDataTenantStatus.MasterFolderHasNoSubfolder => ResponseMessage.MasterFolderHasNoSubfolder,
            _ => string.Empty
        };
    }
}
