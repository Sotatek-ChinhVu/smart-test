using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SearchTenMstItem
{
    public class SearchTenMstItemOutputData : IOutputData
    {
        public SearchTenMstItemOutputData(List<TenItemModel> tenMsts, int totalCount, SearchTenMstItemStatus status)
        {
            TenMsts = tenMsts;
            TotalCount = totalCount;
            Status = status;
        }

        public List<TenItemModel> TenMsts { get; private set; }
        public int TotalCount { get; private set; }
        public SearchTenMstItemStatus Status { get; private set; }
    }
}
