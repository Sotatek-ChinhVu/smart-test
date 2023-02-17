using Domain.Models.Diseases;

namespace EmrCloudApi.Responses.Diseases
{
    public class GetTreeSetByomeiResponse
    {
        public GetTreeSetByomeiResponse(IEnumerable<ByomeiSetMstModel> datas)
        {
            Datas = datas;
        }

        public IEnumerable<ByomeiSetMstModel> Datas { get; private set; } = new List<ByomeiSetMstModel>();
    }
}
