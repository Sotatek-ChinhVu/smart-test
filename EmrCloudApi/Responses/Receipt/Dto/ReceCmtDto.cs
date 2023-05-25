using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt.Dto;

public class ReceCmtDto
{
    public ReceCmtDto(ReceCmtItem output)
    {
        Id = output.Id;
        PtId = output.PtId;
        SeqNo = output.SeqNo;
        SinYm = output.SinYm;
        HokenId = output.HokenId;
        CmtKbn = output.CmtKbn;
        CmtSbt = output.CmtSbt;
        Cmt = output.Cmt;
        CmtData = output.CmtData;
        ItemCd = output.ItemCd;
        CmtCol1 = output.CmtCol1;
        CmtCol2 = output.CmtCol2;
        CmtCol3 = output.CmtCol3;
        CmtCol4 = output.CmtCol4;
        CmtColKeta1 = output.CmtColKeta1;
        CmtColKeta2 = output.CmtColKeta2;
        CmtColKeta3 = output.CmtColKeta3;
        CmtColKeta4 = output.CmtColKeta4;
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
}
