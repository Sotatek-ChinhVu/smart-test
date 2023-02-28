namespace Domain.Models.Receipt;

public class SyoukiInfModel
{
    public SyoukiInfModel(long ptId, int sinYm, int hokenId, int seqNo, int sortNo, int syoukiKbn, string syouki)
    {
        PtId = ptId;
        SinYm = sinYm;
        HokenId = hokenId;
        SeqNo = seqNo;
        SortNo = sortNo;
        SyoukiKbn = syoukiKbn;
        Syouki = syouki;
        IsDeleted = false;
    }

    public SyoukiInfModel(long ptId, int sinYm, int hokenId, int seqNo, int sortNo, int syoukiKbn, string syouki, bool isDeleted)
    {
        PtId = ptId;
        SinYm = sinYm;
        HokenId = hokenId;
        SeqNo = seqNo;
        SortNo = sortNo;
        SyoukiKbn = syoukiKbn;
        Syouki = syouki;
        IsDeleted = isDeleted;
    }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int HokenId { get; private set; }

    public int SeqNo { get; private set; }

    public int SortNo { get; private set; }

    public int SyoukiKbn { get; private set; }

    public string Syouki { get; private set; }

    public bool IsDeleted { get; private set; }
}
