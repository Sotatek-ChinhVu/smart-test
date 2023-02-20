using Domain.Models.Diseases;

namespace EmrCloudApi.Responses.Diseases
{
    public class GetSetByomeiTreeResponse
    {
        public GetSetByomeiTreeResponse(IEnumerable<ByomeiSetMstModel> datas)
        {
            Datas = datas;
        }

        public IEnumerable<ByomeiSetMstModel> Datas { get; private set; } = new List<ByomeiSetMstModel>();
    }
}
