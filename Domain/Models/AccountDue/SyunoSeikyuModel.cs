using Domain.Models.Accounting;

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
        SeikyuDetail = string.Empty;
        NewSeikyuDetail = string.Empty;
    }

    public SyunoSeikyuModel(int hpId, long ptId, int sinDate, long raiinNo, int nyukinKbn, int seikyuTensu, int adjustFutan, int seikyuGaku, string seikyuDetail, int newSeikyuTensu, int newAdjustFutan, int newSeikyuGaku, string newSeikyuDetail, SyunoRaiinInfModel raiinInfModel, List<SyunoNyukinModel> syunoNyukinModels, List<KaikeiInfModel> kaikeiInfModels, int hokenId)
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
        RaiinInfModel = raiinInfModel;
        SyunoNyukinModels = syunoNyukinModels;
        KaikeiInfModels = kaikeiInfModels;
        HokenId = hokenId;
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

    public SyunoRaiinInfModel RaiinInfModel { get; private set; }

    public List<SyunoNyukinModel> SyunoNyukinModels { get; private set; }

    public List<KaikeiInfModel> KaikeiInfModels { get; private set; }

    public int HokenId { get; private set; }

    #region BindData
    public int TotalPointOne { get => SeikyuTensu; }
    public int KanFutanOne
    {
        get
        {
            var result = KaikeiInfModels?.Where(x => x.RaiinNo == RaiinNo)?.Sum(item => item.PtFutan + item.AdjustRound);
            return result ?? 0;
        }
    }
    public int TotalSelfExpenseOne
    {
        get
        {
            var result = KaikeiInfModels?.Where(x => x.RaiinNo == RaiinNo).Sum(item => item.JihiFutan + item.JihiOuttax);
            return result ?? 0;
        }
    }
    public int TaxOne
    {
        get
        {
            var result = KaikeiInfModels?.Where(x => x.RaiinNo == RaiinNo).Sum(item => item.JihiTax + item.JihiOuttax);
            return result ?? 0;
        }
    }
    public int AdjustFutanOne { get => AdjustFutan; }
    public int SumAdjustOne { get => SeikyuGaku; }
    #endregion
}
