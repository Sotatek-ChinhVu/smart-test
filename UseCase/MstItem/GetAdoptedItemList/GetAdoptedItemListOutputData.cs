using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetAdoptedItemList
{
    public class GetAdoptedItemListOutputData : IOutputData
    {
        public GetAdoptedItemListOutputData(List<TenMstItem> tenMstItems, GetAdoptedItemListStatus status)
        {
            TenMstItems = tenMstItems;
            Status = status;
        }

        public List<TenMstItem> TenMstItems { get; private set; }
        public GetAdoptedItemListStatus Status { get; private set; }
    }
}
