using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SearchTenItem
{
    public class SearchTenItemOutputData: IOutputData
    {
        public SearchTenItemOutputData(List<TenItemModel> listInputModel, int totalCount, SearchTenItemStatus status)
        {
            ListInputModel = listInputModel;
            Status = status;
            TotalCount = totalCount;
        }

        public List<TenItemModel> ListInputModel { get; private set; }
        public int TotalCount { get; private set; }
        public SearchTenItemStatus Status { get; private set; }

    }
}
