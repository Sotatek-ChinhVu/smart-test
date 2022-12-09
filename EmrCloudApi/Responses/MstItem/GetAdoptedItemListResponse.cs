using UseCase.MstItem.GetAdoptedItemList;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetAdoptedItemListResponse
    {
        public GetAdoptedItemListResponse(List<TenMstItem> tenMstItems)
        {
            TenMstItems = tenMstItems;
        }
        public List<TenMstItem> TenMstItems { get; private set; }
    }
}
