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
}
