using Domain.Models.RsvInf;

namespace EmrCloudApi.Responses.MainMenu
{
    public class GetRsvInfToConfirmResponse
    {
        public GetRsvInfToConfirmResponse(List<RsvInfToConfirmModel> rsvInfToConfirms)
        {
            RsvInfToConfirms = rsvInfToConfirms;
        }

        public List<RsvInfToConfirmModel> RsvInfToConfirms { get; private set; }
    }
}
