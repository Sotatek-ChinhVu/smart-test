using EmrCloudApi.Tenant.Responses.MedicalExamination;

namespace EmrCloudApi.Tenant.Responses.NextOrder
{
    public class UpsertOrderInfItemResponse
    {
        public UpsertOrderInfItemResponse(int nextOrderPosition, List<ValidationTodayOrdItemResponse> validationOdrs)
        {
            NextOrderPosition = nextOrderPosition;
            ValidationOdrs = validationOdrs;
        }

        public int NextOrderPosition { get; private set; }
        public List<ValidationTodayOrdItemResponse> ValidationOdrs { get; private set; }
    }
}
