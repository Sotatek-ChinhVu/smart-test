namespace Reporting.Accounting.Model;

public class CoKaikeiInfDailyModel
{
    private List<CoSystemGenerationConfModel> systemGenerationConfs;
    public List<TaxSum> TaxSums { get; }

    public CoKaikeiInfDailyModel(
        int hpId, long ptId, int sinDate, int seikyuGaku, int nyukinKonkai, int nyukinZenkai, int nyukinAdjust,
        int ptFutan, int adjustFutan, int adjustRound, int jihiFutan, int jihiOuttax, int jihiTax,
        int tensu, int misyu, int ptmisyu,
        int jihiFutanTaxFree, int jihiFutanOuttaxNr, int jihiFutanOuttaxGen, int jihiFutanTaxNr, int jihiFutanTaxGen,
        int jihiOuttaxNr, int jihiOuttaxGen, int jihiTaxNr, int jihiTaxGen,
        List<CoSystemGenerationConfModel> systemGenerationConfModels)
    {
        HpId = hpId;
        PtId = ptId;
        SinDate = sinDate;
        SeikyuGaku = seikyuGaku;
        NyukinKonkai = nyukinKonkai;
        NyukinZenkai = nyukinZenkai;
        TotalNyukinAdjust = nyukinAdjust;
        PtFutan = ptFutan;
        AdjustFutan = adjustFutan;
        AdjustRound = adjustRound;
        JihiFutan = jihiFutan;
        JihiOuttax = jihiOuttax;
        JihiTax = jihiTax;
        JihiFutanTaxFree = jihiFutanTaxFree;
        JihiFutanOuttaxNr = jihiFutanOuttaxNr;
        JihiFutanOuttaxGen = jihiFutanOuttaxGen;
        JihiFutanTaxNr = jihiFutanTaxNr;
        JihiFutanTaxGen = jihiFutanTaxGen;
        JihiOuttaxNr = jihiOuttaxNr;
        JihiOuttaxGen = jihiOuttaxGen;
        JihiTaxNr = jihiTaxNr;
        JihiTaxGen = jihiTaxGen;

        Tensu = tensu;
        Misyu = misyu;
        PtMisyu = ptmisyu;

        systemGenerationConfs = systemGenerationConfModels;

        // 税率別の金額を集計する
        TaxSums = new List<TaxSum>();

        List<CoSystemGenerationConfModel> taxConfs;
        taxConfs =
            systemGenerationConfModels.FindAll(p => p.GrpEdaNo == 0 && p.StartDate <= SinDate && p.EndDate >= SinDate);

        if (taxConfs.Any())
        {
            AddTaxSum(taxConfs.First().Val, JihiFutanOuttaxNr, JihiFutanTaxNr, JihiOuttaxNr, JihiTaxNr);
        }

        taxConfs =
            systemGenerationConfModels.FindAll(p => p.GrpEdaNo == 1 && p.StartDate <= SinDate && p.EndDate >= SinDate);

        if (taxConfs.Any())
        {
            AddTaxSum(taxConfs.First().Val, JihiFutanOuttaxGen, JihiFutanTaxGen, JihiOuttaxGen, JihiTaxGen);
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
            TaxSum taxSum = new();
            if (TaxSums.Any(p => p.Rate == Arate))
            {
                taxSum = TaxSums.Find(p => p.Rate == Arate);
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

    /// <summary>
    /// 医療機関識別ID
    /// </summary>
    public int HpId { get; set; }
    /// <summary>
    /// 患者ID
    /// </summary>
    public long PtId { get; set; }
    /// <summary>
    /// 診療日
    /// </summary>
    public int SinDate { get; set; }
    /// <summary>
    /// 請求額
    /// </summary>
    public int SeikyuGaku { get; set; }
    /// <summary>
    /// 今回入金額
    /// </summary>
    public int NyukinKonkai { get; set; }
    /// <summary>
    /// 前回入金額
    /// </summary>
    public int NyukinZenkai { get; set; }
    /// <summary>
    /// 患者負担額
    /// </summary>
    public int PtFutan { get; set; }
    public int AdjustFutan { get; set; }
    public int AdjustRound { get; set; }
    public int TotalNyukinAdjust { get; set; }
    /// <summary>
    /// 自費負担額
    /// </summary>
    public int JihiFutan { get; set; }
    /// <summary>
    /// 自費項目金額合計
    /// </summary>
    public int JihiKoumokuAll { get; set; }
    /// <summary>
    /// 自費外税
    /// </summary>
    public int JihiOuttax { get; set; }
    /// <summary>
    /// 自費内税
    /// </summary>
    public int JihiTax { get; set; }
    /// <summary>
    /// 診療点数
    /// </summary>
    public int Tensu { get; set; }
    public int Misyu { get; set; }
    public int PtMisyu { get; set; }
    public int JihiFutanTaxFree { get; set; }
    public int JihiFutanOuttaxNr { get; set; }
    public int JihiFutanOuttaxGen { get; set; }
    public int JihiFutanTaxNr { get; set; }
    public int JihiFutanTaxGen { get; set; }
    public int JihiOuttaxNr { get; set; }
    public int JihiOuttaxGen { get; set; }
    public int JihiTaxNr { get; set; }
    public int JihiTaxGen { get; set; }
}
