using Domain.Models.Insurance;

namespace EmrCloudApi.Responses.Insurance
{
    public class GetKohiPriorityListResponse
    {
        public GetKohiPriorityListResponse(List<KohiPriorityModel> data)
        {
            Data = data;
        }

        public List<KohiPriorityModel> Data { get; private set; } = new List<KohiPriorityModel>();
    }
}
