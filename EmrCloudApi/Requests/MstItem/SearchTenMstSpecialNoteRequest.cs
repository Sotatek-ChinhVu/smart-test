using UseCase.MstItem.SearchTenMstItemSpecialNote;

namespace EmrCloudApi.Requests.MstItem
{
    public class SearchTenMstSpecialNoteRequest
    {
        public int HpId { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public SearchItemCondition ItemCondition { get; set; } = new();
    }
}
