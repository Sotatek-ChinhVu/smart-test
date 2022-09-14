using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SearchTenItem
{
    public class SearchTenItemOutputData: IOutputData
    {
        public SearchTenItemOutputData(List<TenItemModel> listInputModel, SearchTenItemStatus status)
        {
            ListInputModel = listInputModel;
            Status = status;
        }

        public List<TenItemModel> ListInputModel { get; private set; }

        public SearchTenItemStatus Status { get; private set; }

    }
}
