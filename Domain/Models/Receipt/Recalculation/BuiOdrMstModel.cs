namespace Domain.Models.Receipt.Recalculation;

public class BuiOdrMstModel
{
    public BuiOdrMstModel(int buiId, string odrBui, int lrKbn, int mustLrKbn, int bothKbn, int koui30, int koui40, int koui50, int koui60, int koui70, int koui80)
    {
        BuiId = buiId;
        OdrBui = odrBui;
        LrKbn = lrKbn;
        MustLrKbn = mustLrKbn;
        BothKbn = bothKbn;
        Koui30 = koui30;
        Koui40 = koui40;
        Koui50 = koui50;
        Koui60 = koui60;
        Koui70 = koui70;
        Koui80 = koui80;
    }

    public int BuiId { get; private set; }

    public string OdrBui { get; private set; }

    public int LrKbn { get; private set; }

    public int MustLrKbn { get; private set; }

    public int BothKbn { get; private set; }

    public int Koui30 { get; private set; }

    public int Koui40 { get; private set; }

    public int Koui50 { get; private set; }

    public int Koui60 { get; private set; }

    public int Koui70 { get; private set; }

    public int Koui80 { get; private set; }
}
