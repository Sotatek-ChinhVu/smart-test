using UseCase.Releasenote.UpdateListReleasenote;

namespace EmrCloudApi.Responses.MainMenu
{
    public class UpdateListReleasenoteResponse
    {
        public UpdateListReleasenoteResponse(bool status)
        {
            Status = status;
        }

        public bool Status { get; private set; }
    }
}
