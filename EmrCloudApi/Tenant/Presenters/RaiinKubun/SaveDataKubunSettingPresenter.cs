using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.RaiinKubun;
using UseCase.RaiinKubunMst.Save;

namespace EmrCloudApi.Tenant.Presenters.RaiinKubun
{
    public class SaveDataKubunSettingPresenter : ISaveDataKubunSettingOutputPort
    {
        public Response<SaveDataKubunSettingResponse> Result { get; private set; } = default!;

        public void Complete(SaveDataKubunSettingOutputData outputData)
        {
            var result = outputData.Message.Any();
            Result = new Response<SaveDataKubunSettingResponse>()
            {
                Data = new SaveDataKubunSettingResponse(!result),
                Status = result ? 1 : 0
            };
            Result.Message = outputData.ToString() ?? ResponseMessage.Success;
        }
    }
}
