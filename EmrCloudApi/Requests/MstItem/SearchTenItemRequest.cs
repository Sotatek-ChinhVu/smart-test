using UseCase.MstItem.SearchTenItem;

namespace EmrCloudApi.Requests.MstItem
{
    public class SearchTenItemRequest
    {
        public int HpId { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public SearchItemCondition ItemCondition { get; set; } = new();
    }
}
