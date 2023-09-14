using Domain.Models.ListSetGenerationMst;

namespace EmrCloudApi.Responses.ListSetGenerationMst
{
    public class GetListSetGenerationMstResponse
    {
        public List<ListSetGenerationMstModel> Data { get; set; } = new List<ListSetGenerationMstModel>();
    }
}
