using Domain.Models.Reception;

namespace EmrCloudApi.Tenant.Responses.VisitingList
{
    public class GetReceptionCommentResponse
    {
        public GetReceptionCommentResponse(List<ReceptionModel> receptionModels)
        {
            ReceptionModels = receptionModels;
        }

        public List<ReceptionModel> ReceptionModels { get; set; }
    }
}
