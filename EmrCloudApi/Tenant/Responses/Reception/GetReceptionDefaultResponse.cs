using Domain.Models.Reception;

namespace EmrCloudApi.Tenant.Responses.Reception
{
    public class GetReceptionDefaultResponse
    {
        public GetReceptionDefaultResponse(ReceptionDefautDataModel data)
        {
            Data = data;
        }

        public ReceptionDefautDataModel Data { get; private set; }
    }
}
