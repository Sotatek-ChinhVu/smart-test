using Domain.Models.Reception;

namespace EmrCloudApi.Tenant.Responses.Reception
{
    public class GetReceptionCommentResponse
    {
        public GetReceptionCommentResponse(ReceptionModel receptionModels)
        {
            ReceptionModels = receptionModels;
        }

        public ReceptionModel ReceptionModels { get; set; }
    }
}
