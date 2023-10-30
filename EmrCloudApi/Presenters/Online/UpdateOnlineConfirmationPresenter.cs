using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Online;
using UseCase.Online.SaveOnlineConfirmation;

namespace EmrCloudApi.Presenters.Online
{
    public class UpdateOnlineConfirmationPresenter : IUpdateOnlineConfirmationOutputPort
    {
        public Response<UpdateOnlineConfirmationResponse> Result { get; private set; } = new();
        public void Complete(UpdateOnlineConfirmationOutputData outputData)
        {
            Result.Data = new UpdateOnlineConfirmationResponse(outputData.Status);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(UpdateOnlineConfirmationStatus status) => status switch
        {
            UpdateOnlineConfirmationStatus.Successed => ResponseMessage.Success,
            UpdateOnlineConfirmationStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
