using UseCase.Online.SaveOnlineConfirmation;

namespace EmrCloudApi.Responses.Online
{
    public class UpdateOnlineConfirmationResponse
    {
        public UpdateOnlineConfirmationResponse(string message, UpdateOnlineConfirmationStatus status)
        {
            Message = message;
            Status = status;
        }

        public string Message { get; private set; }

        public UpdateOnlineConfirmationStatus Status { get; private set; }
    }
}
