using Domain.Models.ReceSeikyu;

namespace EmrCloudApi.Responses.ReceSeikyu;

public class RecedenHenJiyuuDto
{
    public RecedenHenJiyuuDto(RecedenHenJiyuuModel model)
    {
        HpId = model.HpId;
        PtId = model.PtId;
        HokenId = model.HokenId;
        SinYm = model.SinYm;
        SeqNo = model.SeqNo;
        HenreiJiyuuCd = model.HenreiJiyuuCd;
        HenreiJiyuu = model.HenreiJiyuu;
        Hosoku = model.Hosoku;
        HokenKbn = model.HokenKbn;
        Houbetu = model.Houbetu;
        HokenSentaku = model.HokenSentaku;
        HokenStartDate = model.HokenStartDate;
        HokenEndDate = model.HokenEndDate;
        HokensyaNo = model.HokensyaNo;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int HokenId { get; private set; }

    public int SinYm { get; private set; }

    public int SeqNo { get; private set; }

    public string HenreiJiyuuCd { get; private set; }

    public string HenreiJiyuu { get; private set; }

    public string Hosoku { get; private set; }

    public int HokenKbn { get; private set; }

    public string Houbetu { get; private set; }

    public string HokenSentaku { get; private set; }

    public int HokenStartDate { get; private set; }

    public int HokenEndDate { get; private set; }

    public string HokensyaNo { get; private set; }
}
