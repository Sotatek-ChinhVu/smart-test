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
            Result.Data = new SaveSwapHokenResponse(outputData.Status , outputData.Message , outputData.Type, outputData.SeikyuYms);
            Result.Status = (int)outputData.Status;
            Result.Message = string.IsNullOrEmpty(outputData.Message) ? GetMessage(outputData.Status) : outputData.Message;
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
