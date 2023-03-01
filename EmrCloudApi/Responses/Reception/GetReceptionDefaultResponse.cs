using Domain.Models.Reception;

namespace EmrCloudApi.Responses.Reception
{
    public class GetReceptionDefaultResponse
    {
        public GetReceptionDefaultResponse(ReceptionDto data)
        {
            Data = data;
        }

        public ReceptionDto Data { get; private set; }
    }
}
