using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SearchOTC
{
    public class SearchOTCInputData : IInputData<SearchOTCOutputData>
    {
        public SearchOTCInputData(int hpId, string searchValue, int pageIndex, int pageSize)
        {
            HpId = hpId;
            SearchValue = searchValue;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public int HpId { get; set; }
        public string SearchValue { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
