using Domain.Models.Receipt;
using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetListSyoukiInf;

public class GetListSyoukiInfOutputData : IOutputData
{
    public GetListSyoukiInfOutputData(List<SyoukiInfModel> listSyoukiInf, List<SyoukiKbnMstModel> listSyoukiKbnMstItem, GetListSyoukiInfStatus status)
    {
        ListSyoukiInf = listSyoukiInf.Select(item => new SyoukiInfItem(item)).ToList();
        ListSyoukiKbnMst = listSyoukiKbnMstItem.Select(item => new SyoukiKbnMstItem(item)).ToList();
        Status = status;
    }

    public List<SyoukiInfItem> ListSyoukiInf { get; private set; }

    public List<SyoukiKbnMstItem> ListSyoukiKbnMst { get; private set; }

    public GetListSyoukiInfStatus Status { get; private set; }
}
