using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SearchSupplement
{
    public class SearchSupplementInputData : IInputData<SearchSupplementOutputData>
    {
        public SearchSupplementInputData(int hpId, string searchValue, int pageIndex, int pageSize)
        {
            SearchValue = searchValue;
            PageIndex = pageIndex;
            PageSize = pageSize;
            HpId = hpId;
        }

        public string SearchValue { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int HpId { get; set; }
    }
}
