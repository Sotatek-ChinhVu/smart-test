using Domain.Models.Diseases;

namespace EmrCloudApi.Responses.Diseases
{
    public class GetTreeSetByomeiResponse
    {
        public IEnumerable<ByomeiSetMstModel> Datas { get; set; } = new List<ByomeiSetMstModel>();
    }
}
