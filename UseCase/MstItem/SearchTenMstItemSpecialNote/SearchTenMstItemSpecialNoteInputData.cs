using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SearchTenMstItemSpecialNote
{
    public class SearchTenMstItemSpecialNoteInputData : IInputData<SearchTenMstItemSpecialNoteOutputData>
    {
        public SearchTenMstItemSpecialNoteInputData(int hpId, int pageIndex, int pageCount, SearchItemCondition itemCondition)
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
