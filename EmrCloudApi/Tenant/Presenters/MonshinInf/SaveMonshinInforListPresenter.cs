using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.MonshinInfor;
using UseCase.MonshinInfor.Save;

namespace EmrCloudApi.Tenant.Presenters.MonshinInf
{
    public class SaveMonshinInforListPresenter
    {
        public Response<SaveMonshinInforListResponse> Result { get; private set; } = new();

        public void Complete(SaveMonshinOutputData output)
        {
            Result.Data = new SaveMonshinInforListResponse(output.Status == SaveMonshinStatus.Success);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(SaveMonshinStatus status) => status switch
        {
            SaveMonshinStatus.Success => ResponseMessage.Success,
            _ => string.Empty
        };
    }
}
