namespace Domain.Models.Receipt;

public class ReceCmtModel
{
    public ReceCmtModel(long id, long ptId, int seqNo, int sinYm, int hokenId, int cmtKbn, int cmtSbt, string cmt, string itemCd)
    {
        Id = id;
        PtId = ptId;
        SeqNo = seqNo;
        SinYm = sinYm;
        HokenId = hokenId;
        CmtKbn = cmtKbn;
        CmtSbt = cmtSbt;
        Cmt = cmt;
        ItemCd = itemCd;
        IsDeleted = false;
    }

    public ReceCmtModel(long id, long ptId, int seqNo, int sinYm, int hokenId, int cmtKbn, int cmtSbt, string cmt, string itemCd, bool isDeleted)
    {
        Id = id;
        PtId = ptId;
        SeqNo = seqNo;
        SinYm = sinYm;
        HokenId = hokenId;
        CmtKbn = cmtKbn;
        CmtSbt = cmtSbt;
        Cmt = cmt;
        ItemCd = itemCd;
        IsDeleted = isDeleted;
    }

    public long Id { get; private set; }

    public long PtId { get; private set; }

    public int SeqNo { get; private set; }

    public int SinYm { get; private set; }

    public int HokenId { get; private set; }

    public int CmtKbn { get; private set; }

    public int CmtSbt { get; private set; }

    public string Cmt { get; private set; }

    public string ItemCd { get; private set; }

    public bool IsDeleted { get; private set; }
}
