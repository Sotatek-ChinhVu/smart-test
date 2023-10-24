using Domain.Models.Receipt;

namespace UseCase.Receipt;

public class ReceCmtItem
{
    public ReceCmtItem(ReceCmtModel model)
    {
        Id = model.Id;
        PtId = model.PtId;
        SeqNo = model.SeqNo;
        SinYm = model.SinYm;
        HokenId = model.HokenId;
        CmtKbn = model.CmtKbn;
        CmtSbt = model.CmtSbt;
        Cmt = model.Cmt;
        CmtData = model.CmtData;
        ItemCd = model.ItemCd;
        CmtCol1 = model.CmtCol1;
        CmtCol2 = model.CmtCol2;
        CmtCol3 = model.CmtCol3;
        CmtCol4 = model.CmtCol4;
        CmtColKeta1 = model.CmtColKeta1;
        CmtColKeta2 = model.CmtColKeta2;
        CmtColKeta3 = model.CmtColKeta3;
        CmtColKeta4 = model.CmtColKeta4;
        IsDeleted = false;
    }

    public ReceCmtItem(ReceCmtModel model, bool isDeleted)
    {
        Id = model.Id;
        PtId = model.PtId;
        SeqNo = model.SeqNo;
        SinYm = model.SinYm;
        HokenId = model.HokenId;
        CmtKbn = model.CmtKbn;
        CmtSbt = model.CmtSbt;
        Cmt = model.Cmt;
        CmtData = model.CmtData;
        ItemCd = model.ItemCd;
        CmtCol1 = model.CmtCol1;
        CmtCol2 = model.CmtCol2;
        CmtCol3 = model.CmtCol3;
        CmtCol4 = model.CmtCol4;
        CmtColKeta1 = model.CmtColKeta1;
        CmtColKeta2 = model.CmtColKeta2;
        CmtColKeta3 = model.CmtColKeta3;
        CmtColKeta4 = model.CmtColKeta4;
        IsDeleted = isDeleted;
    }

    public ReceCmtItem(long id, int seqNo, int cmtKbn, int cmtSbt, string cmt, string cmtData, string itemCd, bool isDeleted)
    {
        Id = id;
        SeqNo = seqNo;
        CmtKbn = cmtKbn;
        CmtSbt = cmtSbt;
        Cmt = cmt;
        CmtData = cmtData;
        ItemCd = itemCd;
        IsDeleted = isDeleted;
        PtId = 0;
        SinYm = 0;
        HokenId = 0;
    }

    public ReceCmtItem ChangeCmtData(string cmtData, string cmt)
    {
        CmtData = cmtData;
        Cmt = cmt;
        return this;
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

    public int CmtCol1 { get; private set; }

    public int CmtCol2 { get; private set; }

    public int CmtCol3 { get; private set; }

    public int CmtCol4 { get; private set; }

    public int CmtColKeta1 { get; private set; }

    public int CmtColKeta2 { get; private set; }

    public int CmtColKeta3 { get; private set; }

    public int CmtColKeta4 { get; private set; }

    public bool IsDeleted { get; private set; }
}
