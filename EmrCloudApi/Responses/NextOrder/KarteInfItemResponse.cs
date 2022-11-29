using EmrCloudApi.Responses.KarteInf;

namespace EmrCloudApi.Responses.NextOrder
{
    public class KarteInfItemResponse
    {
        public KarteInfItemResponse(int nextOrderPosition, ValidationKarteInfResponse validationOdrs)
        {
            NextOrderPosition = nextOrderPosition;
            ValidationOdrs = validationOdrs;
        }

        public int NextOrderPosition { get; private set; }
        public ValidationKarteInfResponse ValidationOdrs { get; private set; }
    }
}
