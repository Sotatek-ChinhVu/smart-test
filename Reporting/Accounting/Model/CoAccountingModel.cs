using CalculateService.Receipt.ViewModels;

namespace Reporting.Accounting.Model;

public class TaxSum
{
    public int Rate { get; set; }
    public double OuttaxFutan { get; set; }
    public double TaxFutan { get; set; }
    public int OuttaxZei { get; set; }
    public int TaxZei { get; set; }

    public TaxSum()
    {

    }
}

public class CoAccountingModel
{
    CoHpInfModel HpInfModel { get; }
    List<CoKaikeiInfModel> KaikeiInfModels { get; }
    CoPtInfModel PtInfModel { get; }
    public List<SinMeiViewModel> SinMeiViewModels { get; }
    List<CoKarteInfModel> KarteInfModels { get; }
    public List<CoOdrInfModel> OdrInfModels { get; }
    public List<CoOdrInfDetailModel> OdrInfDetailModels { get; }
    public List<CoPtByomeiModel> PtByomeiModels { get; }
    public List<CoKaikeiInfDailyModel> KaikeiInfDailyModels { get; set; }
    public List<CoKaikeiInfMonthlyModel> KaikeiInfMonthlyModels { get; set; }
    public List<CoNyukinInfMonthlyModel> NyukinInfMonthlyModels { get; set; }
    public CoPtMemoModel PtMemoModel { get; }

    public List<CoRaiinInfModel> RaiinInfModels { get; }

    public List<TaxSum> TaxSums { get; }

    private List<CoSystemGenerationConfModel> SystemGenerationConfs;

    /// <summary>
    /// 領収証関連情報
    /// </summary>
    /// <param name="hpInfModel">病院情報</param>
    /// <param name="kaikeiInfModels">会計情報</param>
    /// <param name="ptInfModel">患者情報</param>
    /// <param name="sinMeiViewModels">診療情報リスト（月ごとのリスト）</param>
    /// <param name="karteInfModels">所見情報</param>
    /// <param name="odrInfModels">オーダー情報</param>
    /// <param name="odrInfDetailModels">オーダー詳細情報</param>
    /// <param name="ptByomeiModels">病名情報</param>
    /// <param name="ptMemoModel">メモ</param>
    /// <param name="raiinInfModels">来院情報</param>
    /// <param name="systemGenerationConfModels">システム世代マスタ</param>
    public CoAccountingModel(
        CoHpInfModel hpInfModel, List<CoKaikeiInfModel> kaikeiInfModels,
        CoPtInfModel ptInfModel, List<SinMeiViewModel> sinMeiViewModels,
        List<CoKarteInfModel> karteInfModels, List<CoOdrInfModel> odrInfModels, List<CoOdrInfDetailModel> odrInfDetailModels,
        List<CoPtByomeiModel> ptByomeiModels,
        CoPtMemoModel ptMemoModel,
        List<CoRaiinInfModel> raiinInfModels,
        List<CoSystemGenerationConfModel> systemGenerationConfModels)
    {
        HpInfModel = hpInfModel;
        KaikeiInfModels = kaikeiInfModels;
        PtInfModel = ptInfModel;
        SinMeiViewModels = sinMeiViewModels;
        KarteInfModels = karteInfModels;
        OdrInfModels = odrInfModels;
        OdrInfDetailModels = odrInfDetailModels;
        PtByomeiModels = ptByomeiModels;
        PtMemoModel = ptMemoModel;
        RaiinInfModels = raiinInfModels;
        SystemGenerationConfs = systemGenerationConfModels;
        KaikeiInfDailyModels = new();
        KaikeiInfMonthlyModels = new();
        NyukinInfMonthlyModels = new();

        // 日別データを作成する
        MakeKaikeiInfDailyModels();
        // 月別データを作成する
        MakeKaikeiInfMonthlyModels();
        // 入金月別データを作成する
        MakeNyukinInfMonthlyModels();

        // 税率別の金額を集計する
        TaxSums = new List<TaxSum>();
        foreach (CoKaikeiInfModel kaikeiInf in KaikeiInfModels)
        {
            List<CoSystemGenerationConfModel> taxConfs;
            taxConfs =
                systemGenerationConfModels.FindAll(p => p.GrpEdaNo == 0 && p.StartDate <= kaikeiInf.SinDate && p.EndDate >= kaikeiInf.SinDate);

            if (taxConfs.Any())
            {
                AddTaxSum(taxConfs.FirstOrDefault()?.Val ?? 0, kaikeiInf.JihiFutanOutTaxNr, kaikeiInf.JihiFutanTaxNr, kaikeiInf.JihiOuttaxNr, kaikeiInf.JihiTaxNr);
            }

            taxConfs =
                systemGenerationConfModels.FindAll(p => p.GrpEdaNo == 1 && p.StartDate <= kaikeiInf.SinDate && p.EndDate >= kaikeiInf.SinDate);

            if (taxConfs.Any())
            {
                AddTaxSum(taxConfs.FirstOrDefault()?.Val ?? 0, kaikeiInf.JihiFutanOutTaxGen, kaikeiInf.JihiFutanTaxGen, kaikeiInf.JihiOuttaxGen, kaikeiInf.JihiTaxGen);
            }

        }

        #region local method
        void SetTaxSumParam(ref TaxSum AtaxSum, double AouttaxFutan, double AtaxFutan, int AoutTaxZei, int ATaxZei)
        {
            AtaxSum.OuttaxFutan += AouttaxFutan;
            AtaxSum.TaxFutan += AtaxFutan;
            AtaxSum.OuttaxZei += AoutTaxZei;
            AtaxSum.TaxZei += ATaxZei;
        }

        void AddTaxSum(int Arate, double AouttaxFutan, double AtaxFutan, int AoutTaxZei, int ATaxZei)
        {
            TaxSum taxSum = new TaxSum();
            if (TaxSums.Any(p => p.Rate == Arate))
            {
                taxSum = TaxSums.First(p => p.Rate == Arate);
                SetTaxSumParam(ref taxSum, AouttaxFutan, AtaxFutan, AoutTaxZei, ATaxZei);
            }
            else
            {
                taxSum = new TaxSum();
                taxSum.Rate = Arate;
                SetTaxSumParam(ref taxSum, AouttaxFutan, AtaxFutan, AoutTaxZei, ATaxZei);

                TaxSums.Add(taxSum);
            }
        }
        #endregion
    }

    public CoAccountingModel()
    {
        HpInfModel = new();
        KaikeiInfModels = new();
        PtInfModel = new();
        SinMeiViewModels = new();
        KarteInfModels = new();
        OdrInfModels = new();
        OdrInfDetailModels = new();
        PtByomeiModels = new();
        PtMemoModel = new();
        RaiinInfModels = new();
        SystemGenerationConfs = new();
        KaikeiInfDailyModels = new();
        KaikeiInfMonthlyModels = new();
        NyukinInfMonthlyModels = new();
        TaxSums = new();
    }

    private void MakeKaikeiInfDailyModels()
    {
        long preRaiinNo = 0;

        KaikeiInfDailyModels = new List<CoKaikeiInfDailyModel>();
        foreach (CoKaikeiInfModel KaikeiInfModel in KaikeiInfModels.OrderBy(p => p.HpId).ThenBy(p => p.PtId).ThenBy(p => p.SinDate).ThenBy(p => p.SyunoSeikyu?.MaxNyukinDate ?? 0).ThenBy(p => p.RaiinNo))
        {
            if (KaikeiInfDailyModels.Any(p => p.HpId == KaikeiInfModel.HpId && p.PtId == KaikeiInfModel.PtId && p.SinDate == KaikeiInfModel.SinDate))
            {
                CoKaikeiInfDailyModel updKaikeiInfDailyModel =
                    KaikeiInfDailyModels.FindAll(p =>
                        p.HpId == KaikeiInfModel.HpId &&
                        p.PtId == KaikeiInfModel.PtId &&
                        p.SinDate == KaikeiInfModel.SinDate)
                    .First();

                updKaikeiInfDailyModel.SeikyuGaku += KaikeiInfModel.TotalPtFutan;
                updKaikeiInfDailyModel.PtFutan += KaikeiInfModel.PtFutan;
                updKaikeiInfDailyModel.AdjustFutan += KaikeiInfModel.AdjustFutan;
                updKaikeiInfDailyModel.AdjustRound += KaikeiInfModel.AdjustRound;
                updKaikeiInfDailyModel.JihiFutan += KaikeiInfModel.JihiFutan;
                updKaikeiInfDailyModel.JihiOuttax += KaikeiInfModel.JihiOuttax;
                updKaikeiInfDailyModel.JihiTax += KaikeiInfModel.JihiTax;
                updKaikeiInfDailyModel.JihiFutanTaxFree += KaikeiInfModel.JihiFutanTaxFree;
                updKaikeiInfDailyModel.JihiFutanOuttaxNr += KaikeiInfModel.JihiFutanOutTaxNr;
                updKaikeiInfDailyModel.JihiFutanOuttaxGen += KaikeiInfModel.JihiFutanOutTaxGen;
                updKaikeiInfDailyModel.JihiFutanTaxNr += KaikeiInfModel.JihiFutanTaxNr;
                updKaikeiInfDailyModel.JihiFutanTaxGen += KaikeiInfModel.JihiFutanTaxGen;
                updKaikeiInfDailyModel.JihiOuttaxNr += KaikeiInfModel.JihiOuttaxNr;
                updKaikeiInfDailyModel.JihiOuttaxGen += KaikeiInfModel.JihiOuttaxGen;
                updKaikeiInfDailyModel.JihiTaxNr += KaikeiInfModel.JihiTaxNr;
                updKaikeiInfDailyModel.JihiTaxGen += KaikeiInfModel.JihiTaxGen;
                updKaikeiInfDailyModel.PtMisyu += KaikeiInfModel.PtMisyu;

                updKaikeiInfDailyModel.Tensu += KaikeiInfModel.Tensu;

                if (KaikeiInfModel.RaiinNo != preRaiinNo)
                {
                    updKaikeiInfDailyModel.NyukinZenkai += KaikeiInfModel.ExceptLastNyukin;
                    updKaikeiInfDailyModel.NyukinKonkai += KaikeiInfModel.LastNyukin;
                    updKaikeiInfDailyModel.TotalNyukinAdjust += KaikeiInfModel.TotalNyukinAjust;
                }
            }
            else
            {
                CoKaikeiInfDailyModel addKaikeiInfDailyModel =
                    new CoKaikeiInfDailyModel(
                            KaikeiInfModel.HpId,
                            KaikeiInfModel.PtId,
                            KaikeiInfModel.SinDate,
                            KaikeiInfModel.TotalPtFutan,
                            KaikeiInfModel.LastNyukin,
                            KaikeiInfModel.ExceptLastNyukin,
                            KaikeiInfModel.TotalNyukinAjust,
                            KaikeiInfModel.PtFutan,
                            KaikeiInfModel.AdjustFutan,
                            KaikeiInfModel.AdjustRound,
                            KaikeiInfModel.JihiFutan,
                            KaikeiInfModel.JihiOuttax,
                            KaikeiInfModel.JihiTax,
                            KaikeiInfModel.Tensu,
                            KaikeiInfModel.Misyu,
                            KaikeiInfModel.PtMisyu,
                            KaikeiInfModel.JihiFutanTaxFree,
                            KaikeiInfModel.JihiFutanOutTaxNr,
                            KaikeiInfModel.JihiFutanOutTaxGen,
                            KaikeiInfModel.JihiFutanTaxNr,
                            KaikeiInfModel.JihiFutanTaxGen,
                            KaikeiInfModel.JihiOuttaxNr,
                            KaikeiInfModel.JihiOuttaxGen,
                            KaikeiInfModel.JihiTaxNr,
                            KaikeiInfModel.JihiTaxGen,
                            SystemGenerationConfs
                        );
                KaikeiInfDailyModels.Add(addKaikeiInfDailyModel);
            }

            preRaiinNo = KaikeiInfModel.RaiinNo;
        }
    }
    private void MakeKaikeiInfMonthlyModels()
    {
        long preRaiinNo = 0;

        KaikeiInfMonthlyModels = new List<CoKaikeiInfMonthlyModel>();
        foreach (CoKaikeiInfModel KaikeiInfModel in KaikeiInfModels.OrderBy(p => p.HpId).ThenBy(p => p.PtId).ThenBy(p => p.SinDate).ThenBy(p => p.RaiinNo))
        {
            if (KaikeiInfMonthlyModels.Any(p => p.HpId == KaikeiInfModel.HpId && p.PtId == KaikeiInfModel.PtId && p.SinYm == KaikeiInfModel.SinDate / 100))
            {
                CoKaikeiInfMonthlyModel updKaikeiInfMonthlyModel =
                    KaikeiInfMonthlyModels.FindAll(p =>
                        p.HpId == KaikeiInfModel.HpId &&
                        p.PtId == KaikeiInfModel.PtId &&
                        p.SinYm == KaikeiInfModel.SinDate / 100)
                    .First();

                updKaikeiInfMonthlyModel.SeikyuGaku += KaikeiInfModel.TotalPtFutan;
                updKaikeiInfMonthlyModel.PtFutan += KaikeiInfModel.PtFutan;
                updKaikeiInfMonthlyModel.AdjustFutan += KaikeiInfModel.AdjustFutan;
                updKaikeiInfMonthlyModel.AdjustRound += KaikeiInfModel.AdjustRound;
                updKaikeiInfMonthlyModel.JihiFutan += KaikeiInfModel.JihiFutan;
                updKaikeiInfMonthlyModel.JihiOuttax += KaikeiInfModel.JihiOuttax;
                updKaikeiInfMonthlyModel.JihiTax += KaikeiInfModel.JihiTax;
                updKaikeiInfMonthlyModel.Tensu += KaikeiInfModel.Tensu;

                if (KaikeiInfModel.RaiinNo != preRaiinNo)
                {
                    updKaikeiInfMonthlyModel.NyukinKonkai += KaikeiInfModel.LastNyukin;
                    updKaikeiInfMonthlyModel.NyukinZenkai += KaikeiInfModel.ExceptLastNyukin;
                    updKaikeiInfMonthlyModel.TotalNyukin += KaikeiInfModel.TotalNyukin;
                }
            }
            else
            {
                CoKaikeiInfMonthlyModel addKaikeiInfMonthlyModel =
                    new CoKaikeiInfMonthlyModel(
                            KaikeiInfModel.HpId,
                            KaikeiInfModel.PtId,
                            KaikeiInfModel.SinDate / 100,
                            KaikeiInfModel.TotalPtFutan,
                            KaikeiInfModel.LastNyukin,
                            KaikeiInfModel.ExceptLastNyukin,
                            KaikeiInfModel.PtFutan,
                            KaikeiInfModel.AdjustFutan,
                            KaikeiInfModel.AdjustRound,
                            KaikeiInfModel.JihiFutan,
                            KaikeiInfModel.JihiOuttax,
                            KaikeiInfModel.JihiTax,
                            KaikeiInfModel.Tensu,
                            KaikeiInfModel.TotalNyukin
                        );
                KaikeiInfMonthlyModels.Add(addKaikeiInfMonthlyModel);
            }

            preRaiinNo = KaikeiInfModel.RaiinNo;
        }
    }

    /// <summary>
    /// 月別入金額データ生成
    /// </summary>
    private void MakeNyukinInfMonthlyModels()
    {
        NyukinInfMonthlyModels = new List<CoNyukinInfMonthlyModel>();

        foreach (CoKaikeiInfModel kaikeiInf in KaikeiInfModels)
        {
            foreach (CoSyunoNyukinModel nyukinInf in kaikeiInf.SyunoSeikyu.SyunoNyukins)
            {
                List<CoNyukinInfMonthlyModel> nyukinMonth = NyukinInfMonthlyModels.FindAll(p => p.NyukinYm == nyukinInf.NyukinDate / 100);

                if (nyukinMonth.Any())
                {
                    nyukinMonth.First().NyukinGaku += nyukinInf.NyukinGaku;
                    nyukinMonth.First().TotalAdjust += nyukinInf.AdjustFutan;
                }
                else
                {
                    NyukinInfMonthlyModels.Add(new CoNyukinInfMonthlyModel(nyukinInf.HpId, nyukinInf.PtId, nyukinInf.NyukinDate / 100, nyukinInf.NyukinGaku, nyukinInf.AdjustFutan));
                }
            }
        }
    }

    public int HpId
    {
        get => HpInfModel.HpId;
    }
    
    public string HpName
    {
        get => HpInfModel.HpName;
    }
    /// <summary>
    /// 医療機関住所
    /// </summary>
    public string HpAddress
    {
        get => HpInfModel.Address1 + HpInfModel.Address2;
    }
    public string HpAddress1
    {
        get => HpInfModel.Address1;
    }
    public string HpAddress2
    {
        get => HpInfModel.Address2;
    }
    /// <summary>
    /// 医療機関郵便番号
    /// </summary>
    public string HpPostCd
    {
        get => HpInfModel.PostCd;
    }
    public string HpPostCdDsp
    {
        get => HpInfModel.PostCdDsp;
    }
    /// <summary>
    /// 開設者氏名
    /// </summary>
    public string KaisetuName
    {
        get => HpInfModel.KaisetuName;
    }
    /// <summary>
    /// 医療機関電話番号
    /// </summary>
    public string HpTel
    {
        get => HpInfModel.Tel;
    }
    /// <summary>
    /// 医療機関FAX番号
    /// </summary>
    public string HpFaxNo
    {
        get => HpInfModel.FaxNo;
    }
    /// <summary>
    /// 医療機関その他連絡先
    /// </summary>
    public string HpOtherContacts
    {
        get => HpInfModel.OtherContacts;
    }
    /// <summary>
    /// 患者番号
    /// </summary>
    public long PtNum
    {
        get => PtInfModel.PtNum;
    }
    /// <summary>
    /// 患者ID
    /// </summary>
    public long PtId
    {
        get => PtInfModel.PtId;
    }
    /// <summary>
    /// 患者氏名
    /// </summary>
    public string PtName
    {
        get => PtInfModel.Name;
    }
    /// <summary>
    /// 患者カナ氏名
    /// </summary>
    public string PtKanaName
    {
        get => PtInfModel.KanaName;
    }

    /// <summary>
    /// 患者性別
    /// </summary>
    public string PtSex
    {
        get
        {
            string ret = "男";

            if (PtInfModel.Sex == 2)
            {
                ret = "女";
            }

            return ret;
        }
    }

    /// <summary>
    /// 患者住所
    /// </summary>
    public string PtAddress
    {
        get => PtInfModel.HomeAddress1 + PtInfModel.HomeAddress2;
    }

    public string PtAddress1
    {
        get => PtInfModel.HomeAddress1;
    }

    public string PtAddress2
    {
        get => PtInfModel.HomeAddress2;
    }

    /// <summary>
    /// 患者郵便番号
    /// </summary>
    public string PtPostCd
    {
        get => PtInfModel.HomePost;
    }
    public string PtPostCdDsp
    {
        get => PtInfModel.HomePostDsp;
    }
    /// <summary>
    /// 患者生年月日
    /// </summary>
    public int BirthDay
    {
        get => PtInfModel.Birthday;
    }

    /// <summary>
    /// 保険の種類
    /// </summary>
    public string HokenSbt
    {
        get => KaikeiInfModels?.FirstOrDefault()?.HokenSyu ?? string.Empty;
    }

    /// <summary>
    /// 保険の種類（１つでも異なる保険があれば空文字を返す）
    /// </summary>
    public string HokenSbtAll
    {
        get
        {
            string ret = string.Empty;

            foreach (CoKaikeiInfModel kaikeiInfModel in KaikeiInfModels)
            {
                if (ret == string.Empty)
                {
                    ret = kaikeiInfModel.HokenSyu;
                }
                else if (ret != kaikeiInfModel.HokenSyu)
                {
                    ret = string.Empty;
                    break;
                }
            }

            return ret;
        }
    }

    /// <summary>
    /// 負担率
    /// </summary>
    public int? FutanRate
    {
        get => KaikeiInfModels.FirstOrDefault()?.FutanRate;
    }
    /// <summary>
    /// 負担率（１つでも異なる保険があればnullを返す）
    /// </summary>
    public int? FutanRateAll
    {
        get
        {
            int? ret = null;

            foreach (var kaikeiInfModel in KaikeiInfModels ?? new())
            {
                if (ret == null)
                {
                    ret = kaikeiInfModel.FutanRate;
                }
                else if (ret != kaikeiInfModel.FutanRate)
                {
                    ret = null;
                    break;
                }
            }

            return ret;
        }
    }

    /// <summary>
    /// 主保険負担率
    /// </summary>
    public int? HokenRate
    {
        get => KaikeiInfModels.FirstOrDefault()?.HokenRate;
    }

    /// <summary>
    /// 主保険負担率
    /// </summary>
    public int? HokenRateAll
    {
        get
        {
            int? ret = null;

            foreach (CoKaikeiInfModel kaikeiInfModel in KaikeiInfModels)
            {
                if (ret == null)
                {
                    ret = kaikeiInfModel.HokenRate;
                }
                else if (ret != kaikeiInfModel.HokenRate)
                {
                    ret = null;
                    break;
                }
            }

            return ret;
        }
    }

    /// <summary>
    /// 診療点数
    /// </summary>
    public int SinryoTensu
    {
        get
        {
            int ret = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                ret = KaikeiInfModels.Sum(p => p.Tensu);
            }

            return ret;
        }
    }

    /// <summary>
    /// 自費負担額
    /// </summary>
    public int JihiFutan
    {
        get
        {
            int ret = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                ret = KaikeiInfModels.Sum(p => p.JihiFutan);
            }

            return ret;
        }
    }
    /// <summary>
    /// 自費負担額（非課税分）
    /// </summary>
    public int JihiFutanTaxFree
    {
        get
        {
            int ret = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                ret = KaikeiInfModels.Sum(p => p.JihiFutanTaxFree);
            }

            return ret;
        }
    }
    /// <summary>
    /// 自費負担額（通常税率外税分）
    /// </summary>
    public int JihiFutanOutTaxNr
    {
        get
        {
            int ret = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                ret = KaikeiInfModels.Sum(p => p.JihiFutanOutTaxNr);
            }

            return ret;
        }
    }
    /// <summary>
    /// 自費負担額（軽減税率外税分）
    /// </summary>
    public int JihiFutanOutTaxGen
    {
        get
        {
            int ret = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                ret = KaikeiInfModels.Sum(p => p.JihiFutanOutTaxGen);
            }

            return ret;
        }
    }
    /// <summary>
    /// 自費負担額（通常税率内税分）
    /// </summary>
    public int JihiFutanTaxNr
    {
        get
        {
            int ret = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                ret = KaikeiInfModels.Sum(p => p.JihiFutanTaxNr);
            }

            return ret;
        }
    }
    /// <summary>
    /// 自費負担額（軽減税率内税分）
    /// </summary>
    public int JihiFutanTaxGen
    {
        get
        {
            int ret = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                ret = KaikeiInfModels.Sum(p => p.JihiFutanTaxGen);
            }

            return ret;
        }
    }

    /// <summary>
    /// 自費内税
    /// </summary>
    public int JihiTax
    {
        get
        {
            int ret = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                ret = KaikeiInfModels.Sum(p => p.JihiTax);
            }

            return ret;
        }
    }
    /// <summary>
    /// 自費内税（通常税率）
    /// </summary>
    public int JihiTaxNr
    {
        get
        {
            int ret = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                ret = KaikeiInfModels.Sum(p => p.JihiTaxNr);
            }

            return ret;
        }
    }
    /// <summary>
    /// 自費内税（軽減税率）
    /// </summary>
    public int JihiTaxGen
    {
        get
        {
            int ret = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                ret = KaikeiInfModels.Sum(p => p.JihiTaxGen);
            }

            return ret;
        }
    }

    /// <summary>
    /// 自費外税
    /// </summary>
    public int JihiOuttax
    {
        get
        {
            int ret = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                ret = KaikeiInfModels.Sum(p => p.JihiOuttax);
            }

            return ret;
        }
    }
    /// <summary>
    /// 自費外税（通常税率）
    /// </summary>
    public int JihiOuttaxNr
    {
        get
        {
            int ret = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                ret = KaikeiInfModels.Sum(p => p.JihiOuttaxNr);
            }

            return ret;
        }
    }
    /// <summary>
    /// 自費外税（軽減税率）
    /// </summary>
    public int JihiOuttaxGen
    {
        get
        {
            int ret = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                ret = KaikeiInfModels.Sum(p => p.JihiOuttaxGen);
            }

            return ret;
        }
    }
    /// <summary>
    /// 患者負担額
    /// </summary>
    public int PtFutan
    {
        get
        {
            int ret = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                ret = KaikeiInfModels.Sum(p => p.PtFutan);
            }

            return ret;
        }
    }
    /// <summary>
    /// 調整額
    /// </summary>
    public int AdjustFutan
    {
        get
        {
            int ret = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                ret = KaikeiInfModels.Sum(p => p.AdjustFutan);
            }

            return ret;
        }
    }
    /// <summary>
    /// 調整額
    /// </summary>
    public int AdjustRound
    {
        get
        {
            int ret = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                ret = KaikeiInfModels.Sum(p => p.AdjustRound);
            }

            return ret;
        }
    }
    /// <summary>
    /// 総患者負担額（免除分は除く）
    /// </summary>
    public int TotalPtFutan
    {
        get
        {
            int ret = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                ret = KaikeiInfModels.Where(p => p.NyukinKbn != 2).Sum(p => p.TotalPtFutan);
            }

            return ret;
        }
    }
    /// <summary>
    /// 総患者負担額（免除分を含む）
    /// </summary>
    public int TotalPtFutanIncludeMenjyo
    {
        get
        {
            int ret = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                ret = KaikeiInfModels.Sum(p => p.TotalPtFutan);
            }

            return ret;
        }
    }
    /// <summary>
    /// 最後を除いた入金額
    /// </summary>
    public int ExceptLastNyukin
    {
        get
        {
            int ret = 0;
            long preRaiinNo = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                foreach (CoKaikeiInfModel KaikeiInfModel in KaikeiInfModels.OrderBy(p => p.RaiinNo))
                {
                    if (preRaiinNo != KaikeiInfModel.RaiinNo)
                    {
                        ret += KaikeiInfModel.ExceptLastNyukin;
                        preRaiinNo = KaikeiInfModel.RaiinNo;
                    }
                }
            }

            return ret;
        }
    }
    /// <summary>
    /// 最後の入金額
    /// </summary>
    public int LastNyukin
    {
        get
        {
            int ret = 0;
            long preRaiinNo = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                foreach (CoKaikeiInfModel KaikeiInfModel in KaikeiInfModels.OrderBy(p => p.RaiinNo))
                {
                    if (preRaiinNo != KaikeiInfModel.RaiinNo)
                    {
                        ret += KaikeiInfModel.LastNyukin;
                        preRaiinNo = KaikeiInfModel.RaiinNo;
                    }
                }
            }

            return ret;
        }
    }
    /// <summary>
    /// 総入金額
    /// </summary>
    public int TotalNyukin
    {
        get
        {
            int ret = 0;
            long preRaiinNo = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                foreach (CoKaikeiInfModel KaikeiInfModel in KaikeiInfModels.OrderBy(p => p.RaiinNo))
                {
                    if (preRaiinNo != KaikeiInfModel.RaiinNo)
                    {
                        ret += KaikeiInfModel.TotalNyukin;
                        preRaiinNo = KaikeiInfModel.RaiinNo;
                    }
                }
            }

            return ret;
        }
    }
    /// <summary>
    /// 合計入金調整額（免除分は除く）
    /// </summary>
    public int TotalNyukinAjust
    {
        get
        {
            int ret = 0;
            long preRaiinNo = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                foreach (CoKaikeiInfModel KaikeiInfModel in KaikeiInfModels.Where(p => p.NyukinKbn != 2).OrderBy(p => p.RaiinNo))
                {
                    if (preRaiinNo != KaikeiInfModel.RaiinNo)
                    {
                        ret += KaikeiInfModel.TotalNyukinAjust;
                        preRaiinNo = KaikeiInfModel.RaiinNo;
                    }
                }
            }

            return ret;

        }
    }

    public string PayMethod
    {
        get
        {
            string ret = string.Empty;
            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                List<string> payMethods = new List<string>();
                foreach (CoKaikeiInfModel kaikeiInf in KaikeiInfModels)
                {
                    foreach (string paymethod in kaikeiInf.PayMethod)
                    {
                        if (!payMethods.Any(p => p == paymethod))
                        {
                            payMethods.Add(paymethod);
                        }
                    }
                }

                foreach (string paymethod in payMethods)
                {
                    if (!string.IsNullOrEmpty(ret))
                    {
                        ret += "・";
                    }
                    ret += paymethod;
                }
            }

            return ret;
        }
    }
    /// <summary>
    /// コード区分ごとの点数を集計する
    /// </summary>
    /// <param name="CdKbns"></param>
    /// <returns></returns>
    public double TotalTen(List<string> CdKbns)
    {
        double ret = 0;

        foreach (SinMeiViewModel sinmeiView in SinMeiViewModels)
        {
            ret += (sinmeiView.SinKoui?.Where(p => p.EntenKbn == 0 && CdKbns.Contains(p.CdKbn)).Sum(p => p.TotalTen) ?? 0) +
                         (sinmeiView.SinKoui?.Where(p => p.EntenKbn == 1 && CdKbns.Contains(p.CdKbn)).Sum(p => p.TotalTen / 10) ?? 0);
        }
        return ret;
    }
    public double TotalSinTen(List<string> CdKbns)
    {
        double ret = 0;

        foreach (SinMeiViewModel sinmeiView in SinMeiViewModels)
        {
            ret += (sinmeiView.SinKoui?.Where(p => p.EntenKbn == 0 && CdKbns.Contains(p.CdKbn)).Sum(p => p.TotalTen) ?? 0) +
                         (sinmeiView.SinKoui?.Where(p => p.SanteiKbn != 2 && p.EntenKbn == 1 && CdKbns.Contains(p.CdKbn)).Sum(p => p.TotalTen / 10) ?? 0);
        }
        return ret;
    }
    /// <summary>
    /// 自費種別ごとの金額を集計する
    /// </summary>
    /// <param name="JihiSbts"></param>
    /// <returns></returns>
    public double TotalJihiKingaku(int JihiSbt)
    {
        double ret = 0;

        foreach (SinMeiViewModel sinmeiView in SinMeiViewModels)
        {
            ret += sinmeiView.SinKoui?.Where(p => p.JihiSbt == JihiSbt).Sum(p => p.TotalTen) ?? 0;
        }
        return ret;
    }
    /// <summary>
    /// 自費診療の金額を集計する
    /// </summary>
    /// <returns></returns>
    public double TotalJihiKingakuAll()
    {
        double ret = 0;

        foreach (SinMeiViewModel sinmeiView in SinMeiViewModels)
        {
            ret += sinmeiView.SinKoui?.Where(p => p.CdKbn == "JS").Sum(p => p.TotalTen) ?? 0;
        }
        return ret;
    }
    /// <summary>
    /// 未収金
    /// </summary>
    public int Misyu
    {
        get
        {
            int ret = 0;
            long preRaiinNo = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                foreach (CoKaikeiInfModel KaikeiInfModel in KaikeiInfModels.OrderBy(p => p.RaiinNo))
                {
                    if (preRaiinNo != KaikeiInfModel.RaiinNo)
                    {
                        ret += KaikeiInfModel.Misyu;
                        preRaiinNo = KaikeiInfModel.RaiinNo;
                    }
                }
            }

            return ret;
        }
    }
    /// <summary>
    /// 患者未収（未精算分含む）
    /// </summary>
    public int PtMisyu
    {
        get
        {
            int ret = 0;
            long preRaiinNo = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                foreach (CoKaikeiInfModel KaikeiInfModel in KaikeiInfModels.OrderBy(p => p.RaiinNo))
                {
                    if (preRaiinNo != KaikeiInfModel.RaiinNo)
                    {
                        ret += KaikeiInfModel.PtMisyu;
                        preRaiinNo = KaikeiInfModel.RaiinNo;
                    }
                }
            }

            return ret;
        }
    }
    /// <summary>
    /// 返金
    /// </summary>
    public int Henkin
    {
        get
        {
            int ret = 0;
            long preRaiinNo = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                foreach (CoKaikeiInfModel KaikeiInfModel in KaikeiInfModels.OrderBy(p => p.RaiinNo))
                {
                    if (preRaiinNo != KaikeiInfModel.RaiinNo)
                    {
                        ret += KaikeiInfModel.Henkin;
                        preRaiinNo = KaikeiInfModel.RaiinNo;
                    }
                }
            }

            return ret;
        }
    }
    /// <summary>
    /// 保険区分
    ///		0:自費 
    ///		1:社保 
    ///		2:国保
    ///		
    ///		11:労災(短期給付)
    ///		12:労災(傷病年金)
    ///		13:アフターケア
    ///		14:自賠責
    /// </summary>
    public int HokenKbn
    {
        get
        {
            int ret = 0;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                ret = KaikeiInfModels.FirstOrDefault()?.HokenKbn ?? 0;
            }

            return ret;
        }
    }

    /// <summary>
    /// 保険者番号
    /// </summary>
    public string HokensyaNo
    {
        get
        {
            string ret = string.Empty;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                ret = KaikeiInfModels.FirstOrDefault()?.HokensyaNo ?? string.Empty;
            }

            return ret;
        }
    }
    /// <summary>
    /// 記号番号
    /// </summary>
    public string KigoBango
    {
        get
        {
            string ret = string.Empty;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                ret = KaikeiInfModels.FirstOrDefault()?.KigoBango ?? string.Empty;
            }

            return ret;
        }
    }
    /// <summary>
    /// 労災の交付番号
    /// </summary>
    public string RousaiKofuNo
    {
        get
        {
            string ret = string.Empty;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                ret = KaikeiInfModels.FirstOrDefault()?.RousaiKofuNo ?? string.Empty;
            }

            return ret;
        }
    }
    /// <summary>
    /// 自賠保険会社名
    /// </summary>
    public string JibaiHokenName
    {
        get
        {
            string ret = string.Empty;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                ret = KaikeiInfModels.FirstOrDefault()?.JibaiHokenName ?? string.Empty;
            }

            return ret;
        }
    }
    /// <summary>
    /// 本人家族区分
    /// </summary>
    public string HonKe
    {
        get
        {
            string ret = string.Empty;

            if (KaikeiInfModels != null && KaikeiInfModels.Any())
            {
                ret = KaikeiInfModels.FirstOrDefault()?.Honke ?? string.Empty;
            }

            return ret;
        }
    }
    /// <summary>
    /// 公費負担者番号
    /// </summary>
    public string KohiFutansyaNo(int index)
    {
        string ret = string.Empty;

        if (KaikeiInfModels != null && KaikeiInfModels.Any())
        {
            ret = KaikeiInfModels.FirstOrDefault()?.KohiFutansyaNo(index) ?? string.Empty;
        }

        return ret;
    }
    /// <summary>
    /// 公費受給者番号
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string KohiJyukyusyaNo(int index)
    {
        string ret = string.Empty;

        if (KaikeiInfModels != null && KaikeiInfModels.Any())
        {
            ret = KaikeiInfModels.FirstOrDefault()?.KohiJyukyusyaNo(index) ?? string.Empty;
        }

        return ret;
    }

    public List<string> KarteInfStringList(int karteKbn)
    {
        List<string> ret = new List<string>();

        List<string> filteredKarteInfs =
            KarteInfModels.FindAll(p => p.KarteKbn == karteKbn).GroupBy(p => p.Text).Select(p => p.Key).ToList();

        foreach (string text in filteredKarteInfs)
        {
            string[] del = { "\r\n", "\r", "\n" };
            ret.AddRange(text.Split(del, StringSplitOptions.None));
        }

        return ret;
    }
    public string KarteInfText(int karteKbn)
    {
        string ret = string.Empty;

        List<string> filteredKarteInfs =
            KarteInfModels.FindAll(p => p.KarteKbn == karteKbn).GroupBy(p => p.Text).Select(p => p.Key).ToList();

        foreach (string text in filteredKarteInfs)
        {
            ret += text;
        }

        return ret;
    }

    /// <summary>
    /// 最後の診療日
    /// </summary>
    public int LastSinDate
    {
        get
        {
            int ret = 0;

            if (KaikeiInfDailyModels != null || KaikeiInfDailyModels?.Any() == true)
            {
                ret = KaikeiInfDailyModels.Count > 0 ? KaikeiInfDailyModels.Max(p => p.SinDate) : 0;
            }

            return ret;
        }
    }
}
