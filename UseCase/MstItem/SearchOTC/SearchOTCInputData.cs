using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SearchOTC
{
    public class SearchOTCInputData : IInputData<SearchOTCOutputData>
    {
        public SearchOTCInputData(string searchValue, int pageIndex, int pageSize)
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
