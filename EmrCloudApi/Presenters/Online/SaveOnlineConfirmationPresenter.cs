using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Online;
using UseCase.Online.SaveOnlineConfirmation;

namespace EmrCloudApi.Presenters.Online
{
    public class SaveOnlineConfirmationPresenter : ISaveOnlineConfirmationOutputPort
    {
        public Response<SaveOnlineConfirmationResponse> Result { get; private set; } = new();
        public void Complete(SaveOnlineConfirmationOutputData outputData)
        {
            Result.Data = new SaveOnlineConfirmationResponse(outputData.Status);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(SaveOnlineConfirmationStatus status) => status switch
        {
            SaveOnlineConfirmationStatus.Successed => ResponseMessage.Success,
            SaveOnlineConfirmationStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
