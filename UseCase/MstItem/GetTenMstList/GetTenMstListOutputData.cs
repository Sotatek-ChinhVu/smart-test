using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTenMstList;

public class GetTenMstListOutputData : IOutputData
{
    public GetTenMstListOutputData(List<TenItemModel> tenMstList, GetTenMstListStatus status)
    {
        TenMstList = tenMstList;
        Status = status;
    }

    public List<TenItemModel> TenMstList { get; private set; }

    public GetTenMstListStatus Status { get; private set; }
}
