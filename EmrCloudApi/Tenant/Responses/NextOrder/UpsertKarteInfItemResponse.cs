using EmrCloudApi.Tenant.Responses.KarteInf;

namespace EmrCloudApi.Tenant.Responses.NextOrder
{
    public class UpsertKarteInfItemResponse
    {
        public UpsertKarteInfItemResponse(int nextOrderPosition, ValidationKarteInfResponse validationOdrs)
        {
            NextOrderPosition = nextOrderPosition;
            ValidationOdrs = validationOdrs;
        }

        public int NextOrderPosition { get; private set; }
        public ValidationKarteInfResponse ValidationOdrs { get; private set; }
    }
}
