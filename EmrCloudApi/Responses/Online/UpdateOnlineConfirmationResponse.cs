using UseCase.Online.SaveOnlineConfirmation;

namespace EmrCloudApi.Responses.Online
{
    public class UpdateOnlineConfirmationResponse
    {
        public UpdateOnlineConfirmationResponse(UpdateOnlineConfirmationStatus status)
        {
            Status = status;
        }

        public UpdateOnlineConfirmationStatus Status { get; private set; }
    }
}
