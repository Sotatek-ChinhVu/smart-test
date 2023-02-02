namespace Domain.Models.Receipt;

public class ItemSumModel
{
    public ItemSumModel(long ptId, string itemCd, string itemName, double sum, int sinYm, int hokenId)
    {
        PtId = ptId;
        ItemCd = itemCd;
        ItemName = itemName;
        Sum = sum;
        SinYm = sinYm;
        HokenId = hokenId;
    }

    public ItemSumModel(long ptId, int sinYm, int hokenId)
    {
        PtId = ptId;
        ItemCd = string.Empty;
        ItemName = string.Empty;
        Sum = 0;
        SinYm = sinYm;
        HokenId = hokenId;
    }

    public ItemSumModel()
    {
        PtId = 0;
        ItemCd = string.Empty;
        ItemName = string.Empty;
        Sum = 0;
        SinYm = 0;
        HokenId = 0;
    }

    public long PtId { get; private set; }

    public string ItemCd { get; private set; }

    public string ItemName { get; private set; }

    public double Sum { get; private set; }

    public int SinYm { get; private set; }

    public int HokenId { get; private set; }
}
