using Domain.Models.Reception;

namespace EmrCloudApi.Tenant.Responses.ReceptionVisiting
{
    public class GetReceptionVisitingResponse
    {
        public GetReceptionVisitingResponse(List<ReceptionModel> receptionModels)
        {
            ReceptionModels = receptionModels;
        }

        public List<ReceptionModel> ReceptionModels { get; private set; }
    }
}
