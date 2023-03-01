namespace Domain.Models.Receipt;

public class SyobyoKeikaModel
{
    public SyobyoKeikaModel(long ptId, int sinYm, int sinDay, int hokenId, int seqNo, string keika)
    {
        PtId = ptId;
        SinYm = sinYm;
        SinDay = sinDay;
        HokenId = hokenId;
        SeqNo = seqNo;
        Keika = keika;
        IsDeleted = false;
    }

    public SyobyoKeikaModel(long ptId, int sinYm, int sinDay, int hokenId, int seqNo, string keika, bool isDeleted)
    {
        PtId = ptId;
        SinYm = sinYm;
        SinDay = sinDay;
        HokenId = hokenId;
        SeqNo = seqNo;
        Keika = keika;
        IsDeleted = isDeleted;
    }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int SinDay { get; private set; }

    public int HokenId { get; private set; }

    public int SeqNo { get; private set; }

    public string Keika { get; private set; }

    public bool IsDeleted { get; private set; }
}
