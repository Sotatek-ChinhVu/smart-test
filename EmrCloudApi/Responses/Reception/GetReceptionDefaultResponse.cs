using Domain.Models.Reception;

namespace EmrCloudApi.Responses.Reception
{
    public class GetReceptionDefaultResponse
    {
        public GetReceptionDefaultResponse(ReceptionModel data)
        {
            Data = data;
        }

        public ReceptionModel Data { get; private set; }
    }
}
