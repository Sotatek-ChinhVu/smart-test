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
        RaiinInfModel = new();
        SyunoNyukinModels = new();
        KaikeiInfModels = new();
    }

    public SyunoSeikyuModel()
    {
        SeikyuDetail = string.Empty;
        NewSeikyuDetail = string.Empty;
        RaiinInfModel = new();
        SyunoNyukinModels = new();
        KaikeiInfModels = new();
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
            var kaikeiInfs = KaikeiInfModels?.Where(x => x.RaiinNo == RaiinNo);
            var result = kaikeiInfs?.Count() > 0 ? kaikeiInfs.Sum(item => item.PtFutan + item.AdjustRound) : 0;
            return result;
        }
    }
    public int TotalSelfExpenseOne
    {
        get
        {
            var kaikeiInfs = KaikeiInfModels?.Where(x => x.RaiinNo == RaiinNo);
            var result = kaikeiInfs?.Count() > 0 ? kaikeiInfs.Sum(item => item.JihiFutan + item.JihiOuttax) : 0;
            return result;
        }
    }
    public int TaxOne
    {
        get
        {
            var kaikeiInfs = KaikeiInfModels?.Where(x => x.RaiinNo == RaiinNo);
            var result = kaikeiInfs?.Count() > 0 ? kaikeiInfs.Sum(item => item.JihiTax + item.JihiOuttax) : 0;
            return result;
        }
    }
    public int AdjustFutanOne { get => AdjustFutan; }
    public int SumAdjustOne { get => SeikyuGaku; }
    #endregion
}
