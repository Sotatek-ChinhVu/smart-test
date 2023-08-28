using Domain.Models.Insurance;

namespace EmrCloudApi.Responses.Insurance
{
    public class FindPtHokenListResponse
    {
        public FindPtHokenListResponse(List<HokenInfModel> hokenInfModels)
        {
            HokenInfModels = hokenInfModels;
        }

        public List<HokenInfModel> HokenInfModels { get; private set; }
    }
}
