using Domain.Models.ReceptionComment;

namespace EmrCloudApi.Tenant.Responses.VisitingList
{
    public class GetReceptionCommentResponse
    {
        public GetReceptionCommentResponse(List<ReceptionCommentModel> receptionCommentModels)
        {
            ReceptionCommentModel = receptionCommentModels;
        }

        public List<ReceptionCommentModel> ReceptionCommentModel { get; set; }
    }
}
