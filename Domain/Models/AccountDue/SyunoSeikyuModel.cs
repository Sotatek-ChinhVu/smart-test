namespace Domain.Models.AccountDue;

public class SyunoSeikyuModel
{
    public SyunoSeikyuModel(int hpId, long ptId, int sinDate, long raiinNo, int nyukinKbn, int seikyuTensu, int adjustFutan, int seikyuGaku, string seikyuDetail, int newSeikyuTensu, int newAdjustFutan, int newSeikyuGaku, string newSeikyuDetail)
    {
        HpId = hpId;
        PtId = ptId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        NyukinKbn = nyukinKbn;
        SeikyuTensu = seikyuTensu;
        AdjustFutan = adjustFutan;
        SeikyuGaku = seikyuGaku;
        SeikyuDetail = seikyuDetail;
        NewSeikyuTensu = newSeikyuTensu;
        NewAdjustFutan = newAdjustFutan;
        NewSeikyuGaku = newSeikyuGaku;
        NewSeikyuDetail = newSeikyuDetail;
    }

    public SyunoSeikyuModel()
    {
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public int NyukinKbn { get; private set; }

    public int SeikyuTensu { get; private set; }

    public int AdjustFutan { get; private set; }

    public int SeikyuGaku { get; private set; }

    public string SeikyuDetail { get; private set; }

    public int NewSeikyuTensu { get; private set; }

    public int NewAdjustFutan { get; private set; }

    public int NewSeikyuGaku { get; private set; }

    public string NewSeikyuDetail { get; private set; }
}
