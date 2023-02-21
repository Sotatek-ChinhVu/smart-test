using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SwapHoken;
using UseCase.SwapHoken.Save;

namespace EmrCloudApi.Presenters.SwapHoken
{
    public class SaveSwapHokenPresenter : ISaveSwapHokenOutputPort
    {
        public Response<SaveSwapHokenResponse> Result { get; private set; } = new Response<SaveSwapHokenResponse>();

        public void Complete(SaveSwapHokenOutputData outputData)
        {
            Result.Data = new SaveSwapHokenResponse(outputData.Status , outputData.Message , outputData.Type);
            Result.Status = (int)outputData.Status;
            Result.Message = outputData.Message ?? GetMessage(outputData.Status);
        }

        private string GetMessage(SaveSwapHokenStatus status) => status switch
        {
            SaveSwapHokenStatus.Successful => ResponseMessage.Success,
            SaveSwapHokenStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            SaveSwapHokenStatus.InvalidPtId => ResponseMessage.InvalidPtId,
            SaveSwapHokenStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
