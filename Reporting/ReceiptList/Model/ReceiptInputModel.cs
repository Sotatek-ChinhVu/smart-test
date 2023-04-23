namespace Reporting.ReceiptList.Model;

public class ReceiptInputModel
{
    public ReceiptInputModel(int sinYm, long ptId, int hokenId)
    {
        SinYm = sinYm;
        PtId = ptId;
        HokenId = hokenId;
    }

    public int SinYm { get; set; }

    public long PtId { get; set; }

    public int HokenId { get; set; }
}
