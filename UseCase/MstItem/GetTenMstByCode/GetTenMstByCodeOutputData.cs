using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTenMstByCode
{
    public sealed class GetTenMstByCodeOutputData : IOutputData
    {
        public GetTenMstByCodeOutputData(TenMstModel tenMst, GetTenMstByCodeStatus status)
        {
            TenMst = tenMst;
            Status = status;
        }

        public TenMstModel TenMst { get; private set; }
        public GetTenMstByCodeStatus Status { get; private set; }
    }
}
