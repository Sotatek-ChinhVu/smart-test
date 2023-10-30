using UseCase.MainMenu.RsvInfToConfirm;

namespace EmrCloudApi.Responses.MainMenu
{
    public class GetRsvInfToConfirmResponse
    {
        public GetRsvInfToConfirmResponse(List<RsvInfToConfirmItem> rsvInfToConfirms)
        {
            RsvInfToConfirms = rsvInfToConfirms;
        }

        public List<RsvInfToConfirmItem> RsvInfToConfirms { get; private set; }
    }
}
