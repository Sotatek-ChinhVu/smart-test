using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.RaiinKubun;
using UseCase.RaiinKubunMst.Save;

namespace EmrCloudApi.Presenters.RaiinKubun
{
    public class SaveDataKubunSettingPresenter : ISaveDataKubunSettingOutputPort
    {
        public Response<SaveDataKubunSettingResponse> Result { get; private set; } = default!;

        public void Complete(SaveDataKubunSettingOutputData outputData)
        {
            var result = outputData.Message.Any();
            Result = new Response<SaveDataKubunSettingResponse>()
            {
                Data = new SaveDataKubunSettingResponse(outputData.Message.Contains(ResponseMessage.Success)),
                Status = result ? 1 : 0
            };
            Result.Message = outputData.ToString() ?? ResponseMessage.Success;
        }
    }
}
