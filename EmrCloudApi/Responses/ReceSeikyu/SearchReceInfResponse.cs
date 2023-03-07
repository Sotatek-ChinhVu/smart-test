using Domain.Models.ReceSeikyu;

namespace EmrCloudApi.Responses.ReceSeikyu
{
    public class SearchReceInfResponse
    {
        public SearchReceInfResponse(IEnumerable<RegisterRequestModel> data, string ptNameDisplay)
        {
            Data = data;
            PtNameDisplay = ptNameDisplay;
        }

        public IEnumerable<RegisterRequestModel> Data { get; private set; }

        public string PtNameDisplay { get; private set; }
    }
}
