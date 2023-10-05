using UseCase.Online.SaveOnlineConfirmation;

namespace EmrCloudApi.Responses.Online
{
    public class SaveOnlineConfirmationResponse
    {
        public SaveOnlineConfirmationResponse(SaveOnlineConfirmationStatus status)
        {
            Status = status;
        }

        public SaveOnlineConfirmationStatus Status { get; private set; }
    }
}
