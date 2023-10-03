using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetListResultKensaMst
{
    public class GetListKensaMstInputData : IInputData<GetListKensaMstOuputData>
    {
        public int HpId { get; set; }
        public string Keyword { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public GetListKensaMstInputData(int hpId, string keyword, int pageIndex, int pageSize)
        {
            HpId = hpId;
            Keyword = keyword;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}
