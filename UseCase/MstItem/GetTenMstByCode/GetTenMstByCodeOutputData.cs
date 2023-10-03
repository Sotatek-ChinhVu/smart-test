using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTenMstByCode
{
    public sealed class GetTenMstByCodeOutputData : IOutputData
    {
        public GetTenMstByCodeOutputData(TenItemModel? tenMst, GetTenMstByCodeStatus status)
        {
            TenMst = tenMst;
            Status = status;
        }

        public TenItemModel? TenMst { get; private set; }
        public GetTenMstByCodeStatus Status { get; private set; }
    }
}
