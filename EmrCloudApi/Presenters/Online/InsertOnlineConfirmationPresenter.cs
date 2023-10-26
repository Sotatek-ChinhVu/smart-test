using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Online;
using UseCase.Online.InsertOnlineConfirmation;

namespace EmrCloudApi.Presenters.Online
{
    public class InsertOnlineConfirmationPresenter : IInsertOnlineConfirmationOutputPort
    {
        public Response<InsertOnlineConfirmationResponse> Result { get; private set; } = new();
        public void Complete(InsertOnlineConfirmationOutputData outputData)
        {
            Result.Data = new InsertOnlineConfirmationResponse(outputData.Message);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(InsertOnlineConfirmationStatus status) => status switch
        {
            InsertOnlineConfirmationStatus.Successed => ResponseMessage.Success,
            InsertOnlineConfirmationStatus.Failed => ResponseMessage.Failed,
            InsertOnlineConfirmationStatus.InvalidArbitraryFileIdentifier => ResponseMessage.InvalidArbitraryFileIdentifier,
            _ => string.Empty
        };
    }
}
