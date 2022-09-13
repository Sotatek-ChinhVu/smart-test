using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SearchSupplement
{
    public class SearchSupplementInputData : IInputData<SearchSupplementOutputData>
    {
        public SearchSupplementInputData(string searchValue, int pageIndex, int pageSize)
        {
            SearchValue = searchValue;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public string SearchValue { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
