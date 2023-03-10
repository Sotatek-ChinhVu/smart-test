using Domain.Models.Receipt;

namespace UseCase.Receipt.ReceCmtHistory;

public class ReceCmtHistoryOutputItem
{
    public ReceCmtHistoryOutputItem(int sinYm, string sinYmDisplay, string hokenName, List<ReceCmtModel> receCmtList)
    {
        SinYm = sinYm;
        SinYmDisplay = sinYmDisplay;
        HokenName = hokenName;
        ReceCmtList = receCmtList.Select(item => new ReceCmtItem(item)).ToList();
    }

    public int SinYm { get; private set; }

    public string SinYmDisplay { get; private set; }

    public string HokenName { get; private set; }

    public List<ReceCmtItem> ReceCmtList { get; private set; }
}
