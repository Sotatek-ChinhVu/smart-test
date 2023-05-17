using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SearchTenItem
{
    public class SearchTenItemInputData : IInputData<SearchTenItemOutputData>
    {
        public SearchTenItemInputData(int hpId, int pageIndex, int pageCount, SearchItemCondition itemCondition)
        {
            HpId = hpId;
            PageIndex = pageIndex;
            PageCount = pageCount;
            ItemCondition = itemCondition;
        }

        public int HpId { get; private set; }

        public int PageIndex { get; private set; }

        public int PageCount { get; private set; }

        public SearchItemCondition ItemCondition { get; private set; }
    }
}
