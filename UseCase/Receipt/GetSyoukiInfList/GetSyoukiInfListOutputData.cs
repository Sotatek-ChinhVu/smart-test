using Domain.Models.Receipt;
using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetListSyoukiInf;

public class GetSyoukiInfListOutputData : IOutputData
{
    public GetSyoukiInfListOutputData(List<SyoukiInfModel> syoukiInfList, List<SyoukiKbnMstModel> syoukiKbnMstItemList, GetSyoukiInfListStatus status)
    {
        SyoukiInfList = syoukiInfList.Select(item => new SyoukiInfItem(item)).ToList();
        SyoukiKbnMstList = syoukiKbnMstItemList.Select(item => new SyoukiKbnMstItem(item)).ToList();
        Status = status;
    }

    public List<SyoukiInfItem> SyoukiInfList { get; private set; }

    public List<SyoukiKbnMstItem> SyoukiKbnMstList { get; private set; }

    public GetSyoukiInfListStatus Status { get; private set; }
}
