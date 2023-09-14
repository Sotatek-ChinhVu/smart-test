using Domain.Models.ByomeiSetGenerationMst;

namespace EmrCloudApi.Responses.Diseases
{
    public class GetListByomeiSetGenerationMstResponse
    {
        public List<ByomeiSetGenerationMstModel> Data { get; set; } = new List<ByomeiSetGenerationMstModel>();
    }
}
