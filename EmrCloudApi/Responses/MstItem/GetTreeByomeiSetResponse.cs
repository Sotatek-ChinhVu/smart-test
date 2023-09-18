using UseCase.MstItem.GetTreeByomeiSet;

namespace EmrCloudApi.Responses.MstItem
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
