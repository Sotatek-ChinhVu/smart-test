using Domain.Models.ReceptionVisitingModel;

namespace EmrCloudApi.Tenant.Responses.ReceptionVisiting
{
    public class GetReceptionVisitingResponse
    {
        public GetReceptionVisitingResponse(List<ReceptionVisitingModel> receptionVisitingModels)
        {
            ReceptionVisitingModels = receptionVisitingModels;
        }

        public List<ReceptionVisitingModel> ReceptionVisitingModels { get; private set; }
    }
}
