using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.FindTenMst
{
    public class FindTenMstOutputData : IOutputData
    {
        public FindTenMstOutputData(TenItemModel tenItemModel, FindTenMstStatus status)
        {
            TenItemModel = tenItemModel;
            Status = status;
        }

        public TenItemModel TenItemModel { get; private set; }
        public FindTenMstStatus Status { get; private set; }
    }
}
