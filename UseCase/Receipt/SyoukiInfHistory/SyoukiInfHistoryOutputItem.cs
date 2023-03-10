using Domain.Models.Receipt;

namespace UseCase.Receipt.SyoukiInfHistory;

public class SyoukiInfHistoryOutputItem
{
    public SyoukiInfHistoryOutputItem(int sinYm, string sinYmDisplay, string hokenName, List<SyoukiInfModel> syoukiInfList, List<SyoukiKbnMstModel> syoukiKbnList)
    {
        SinYm = sinYm;
        SinYmDisplay = sinYmDisplay;
        HokenName = hokenName;
        SyoukiInfList = syoukiInfList.Select(item => new SyoukiInfItem(item)).ToList();
        SyoukiKbnList = syoukiKbnList.Select(item => new SyoukiKbnMstItem(item)).ToList();
    }

    public int SinYm { get; private set; }

    public string SinYmDisplay { get; private set; }

    public string HokenName { get; private set; }

    public List<SyoukiInfItem> SyoukiInfList { get; private set; }

    public List<SyoukiKbnMstItem> SyoukiKbnList { get; private set; }
}
