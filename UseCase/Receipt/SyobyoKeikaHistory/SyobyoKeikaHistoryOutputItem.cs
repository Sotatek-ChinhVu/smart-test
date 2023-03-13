using Domain.Models.Receipt;

namespace UseCase.Receipt.SyobyoKeikaHistory;

public class SyobyoKeikaHistoryOutputItem
{
    public SyobyoKeikaHistoryOutputItem(int sinYm, string sinYmDisplay, int hokenId, string hokenName, List<SyobyoKeikaModel> syobyoKeikaList)
    {
        SinYm = sinYm;
        SinYmDisplay = sinYmDisplay;
        HokenId = hokenId;
        HokenName = hokenName;
        SyobyoKeikaList = syobyoKeikaList.Select(item => new SyobyoKeikaItem(item)).ToList();
    }

    public int SinYm { get; private set; }

    public string SinYmDisplay { get; private set; }

    public int HokenId { get; private set; }

    public string HokenName { get; private set; }

    public List<SyobyoKeikaItem> SyobyoKeikaList { get; private set; }
}
