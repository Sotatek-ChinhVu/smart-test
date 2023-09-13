using Domain.Models.Diseases;
using UseCase.Diseases.GetTreeByomeiSet;

namespace EmrCloudApi.Responses.Diseases
{
    public class GetTreeByomeiSetResponse
    {
        public GetTreeByomeiSetResponse(IEnumerable<ByomeiSetMstItem> datas)
        {
            Datas = datas;
        }

        public IEnumerable<ByomeiSetMstItem> Datas { get; private set; } = new List<ByomeiSetMstItem>();
    }
}
