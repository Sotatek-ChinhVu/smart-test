using EmrCalculateApi.ReceFutan.Models;

namespace EmrCalculateApi.Responses
{
    public class GetListReceInfResponse
    {
        public GetListReceInfResponse(List<ReceInfModel> receInfModels)
        {
            ReceInfModels = receInfModels;
        }

        public List<ReceInfModel> ReceInfModels { get; private set; }
    }
}
