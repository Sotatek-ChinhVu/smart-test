using Domain.Models.Reception;

namespace EmrCloudApi.Responses.ReceptionVisiting
{
    public class GetReceptionVisitingResponse
    {
        public GetReceptionVisitingResponse(ReceptionModel receptionModels)
        {
            ReceptionModels = receptionModels;
        }

        public ReceptionModel ReceptionModels { get; private set; }
    }
}
