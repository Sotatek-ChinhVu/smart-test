using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetListResultKensaMst
{
    public class GetListKensaMstInputData : IInputData<GetListKensaMstOuputData>
    {
        public int HpId { get; private set; }

        public string Keyword { get; private set; }

        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        public GetListKensaMstInputData(int hpId, string keyword, int pageIndex, int pageSize)
        {
            HpId = hpId;
            Keyword = keyword;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}
