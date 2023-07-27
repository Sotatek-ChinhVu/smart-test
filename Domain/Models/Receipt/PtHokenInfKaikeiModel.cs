namespace Domain.Models.Receipt;

public class PtHokenInfKaikeiModel
{
    public PtHokenInfKaikeiModel(int hokenId, long ptId, int hokenKbn, string houbetu, int honkeKbn, string hokensyaNo, int hokenStartDate, int hokenEndDate)
    {
        HokenId = hokenId;
        PtId = ptId;
        HokenKbn = hokenKbn;
        Houbetu = houbetu;
        HonkeKbn = honkeKbn;
        HokensyaNo = hokensyaNo;
        HokenStartDate = hokenStartDate;
        HokenEndDate = hokenEndDate;
    }

    public int HokenId { get;private set; }

    public long PtId { get; private set; }

    public int HokenKbn { get; private set; }

    public string Houbetu { get; private set; }

    public int HonkeKbn { get; private set; }

    public string HokensyaNo { get; private set; }

    public int HokenStartDate { get; private set; }

    public int HokenEndDate { get; private set; }
}
