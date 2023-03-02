namespace Domain.Models.Receipt.Recalculation;

public class BuiOdrByomeiMstModel
{
    public BuiOdrByomeiMstModel(int buiId, string byomeiBui)
    {
        BuiId = buiId;
        ByomeiBui = byomeiBui;
    }

    public int BuiId { get; private set; }

    public string ByomeiBui { get; private set; }
}
