using Domain.Models.Reception;

namespace EmrCloudApi.Responses.Reception
{
    public class GetReceptionCommentResponse
    {
        public GetReceptionCommentResponse(ReceptionDto receptionModel)
        {
            ReceptionModel = receptionModel;
        }

        public ReceptionDto ReceptionModel { get; set; }
    }
}
