namespace Domain.Models.Receipt;

public class ReceCmtModel
{
    public ReceCmtModel(long id, long ptId, int seqNo, int sinYm, int hokenId, int cmtKbn, int cmtSbt, string cmt, string cmtData, string itemCd, int cmtCol1, int cmtCol2, int cmtCol3, int cmtCol4, int cmtColKeta1, int cmtColKeta2, int cmtColKeta3, int cmtColKeta4, bool isDeleted)
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
        CmtData = cmtData;
        CmtCol1 = cmtCol1;
        CmtCol2 = cmtCol2;
        CmtCol3 = cmtCol3;
        CmtCol4 = cmtCol4;
        CmtColKeta1 = cmtColKeta1;
        CmtColKeta2 = cmtColKeta2;
        CmtColKeta3 = cmtColKeta3;
        CmtColKeta4 = cmtColKeta4;
        IsDeleted = isDeleted;
    }

    public ReceCmtModel(long id, long ptId, int seqNo, int sinYm, int hokenId, int cmtKbn, int cmtSbt, string cmt, string cmtData, string itemCd, bool isDeleted)
    {
        Id = id;
        PtId = ptId;
        SeqNo = seqNo;
        SinYm = sinYm;
        HokenId = hokenId;
        CmtKbn = cmtKbn;
        CmtSbt = cmtSbt;
        Cmt = cmt;
        CmtData = cmtData;
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

    public string CmtData { get; private set; }

    public string ItemCd { get; private set; }

    public bool IsDeleted { get; private set; }

    public int CmtCol1 { get; private set; }

    public int CmtCol2 { get; private set; }

    public int CmtCol3 { get; private set; }

    public int CmtCol4 { get; private set; }

    public int CmtColKeta1 { get; private set; }

    public int CmtColKeta2 { get; private set; }

    public int CmtColKeta3 { get; private set; }

    public int CmtColKeta4 { get; private set; }
}
