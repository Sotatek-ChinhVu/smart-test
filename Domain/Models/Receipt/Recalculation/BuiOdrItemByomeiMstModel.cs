namespace Domain.Models.Receipt.Recalculation;

public class BuiOdrItemByomeiMstModel
{
    public BuiOdrItemByomeiMstModel(string itemCd, string byomeiBui, int lrKbn, int bothKbn)
    {
        ItemCd = itemCd;
        ByomeiBui = byomeiBui;
        LrKbn = lrKbn;
        BothKbn = bothKbn;
    }

    public string ItemCd { get; private set; }

    public string ByomeiBui { get; private set; }

    public int LrKbn { get; private set; }

    public int BothKbn { get; private set; }
}
