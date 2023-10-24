using Domain.Models.ByomeiSetGenerationMst;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetListByomeiSetGenerationMstResponse
    {
        public List<ByomeiSetGenerationMstModel> Data { get; set; } = new List<ByomeiSetGenerationMstModel>();
    }
}
