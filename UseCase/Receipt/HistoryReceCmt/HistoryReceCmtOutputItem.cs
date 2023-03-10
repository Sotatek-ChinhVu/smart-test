namespace UseCase.Receipt.HistoryReceCmt;

public class HistoryReceCmtOutputItem
{
    public HistoryReceCmtOutputItem(int sinYm, string sinYmDisplay, string hokenName, List<ReceCmtItem> receCmtList)
    {
        SinYm = sinYm;
        SinYmDisplay = sinYmDisplay;
        HokenName = hokenName;
        ReceCmtList = receCmtList;
    }

    public int SinYm { get; private set; }
    
    public string SinYmDisplay { get; private set; }
    
    public string HokenName { get; private set; }
    
    public List<ReceCmtItem> ReceCmtList { get; private set; }
}
