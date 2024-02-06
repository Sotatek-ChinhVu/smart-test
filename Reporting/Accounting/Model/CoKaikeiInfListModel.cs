using Helper.Common;

namespace Reporting.Accounting.Model;

public class CoKaikeiInfListModel
{
    public CoKaikeiInfListModel(
        int hpId, long ptId, long ptNum, string ptName, string ptKanaName, int sex, int birthDay, string postCd, string address1, string address2, string tel1, string tel2, string renrakuTel,
        int seikyuGaku, int nyukinGaku, int nyukinAdjust, int misyu, int tensu, int totalIryohi, int ptFutan, int adjustFutan, int adjustRound,
        int totalPtFutan, int jihiFutan, int jihiOuttax, int jihiTax,
        int jihiFutanFree, int jihiFutanOuttaxNr, int jihiFutanOuttaxGen, int jihiFutanTaxNr, int jihiFutanTaxGen,
        int jihiOuttaxNr, int jihiOuttaxGen, int jihiTaxNr, int jihiTaxGen,
        List<(int santeiKbn, int jihiSbt, double kingaku)> jihiKoumokuDtls, CoPtMemoModel ptMemo, List<CoPtGrpInfModel> ptGrpInfModels, List<TaxSum> taxSums, List<CoJihiSbtKingakuModel> jihiSbtKingakus)
    {
        HpId = hpId;
        PtId = ptId;
        PtNum = ptNum;
        PtName = ptName;
        PtKanaName = ptKanaName;
        Sex = sex;
        BirthDay = birthDay;
        PostCd = postCd;
        Address1 = address1;
        Address2 = address2;
        Tel1 = tel1;
        Tel2 = tel2;
        RenrakuTel = renrakuTel;
        SeikyuGaku = seikyuGaku;
        NyukinGaku = nyukinGaku;
        NyukinAdjust = nyukinAdjust;
        Misyu = misyu;
        Tensu = tensu;
        TotalIryohi = totalIryohi;
        PtFutan = ptFutan;
        AdjustFutan = adjustFutan;
        AdjustRound = adjustRound;
        TotalPtFutan = totalPtFutan;
        JihiFutan = jihiFutan;
        JihiOuttax = jihiOuttax;
        JihiTax = jihiTax;
        JihiKoumoku = jihiKoumokuDtls?.Where(p => p.jihiSbt > 0).Sum(p => p.kingaku) ?? 0;
        JihiKoumokuDtls = jihiKoumokuDtls ?? new();
        JihiSinryo = (int)(JihiFutan - JihiKoumoku);
        PtMemo = ptMemo;
        PtGroupInfModels = ptGrpInfModels;

        JihiFutanFree = jihiFutanFree;
        JihiFutanOuttaxNr = jihiFutanOuttaxNr;
        JihiFutanOuttaxGen = jihiFutanOuttaxGen;
        JihiFutanTaxNr = jihiFutanTaxNr;
        JihiFutanTaxGen = jihiFutanTaxGen;

        JihiOuttaxNr = jihiOuttaxNr;
        JihiOuttaxGen = jihiOuttaxGen;
        JihiTaxNr = jihiTaxNr;
        JihiTaxGen = jihiTaxGen;

        TaxSums = taxSums;
        JihiSbtKingakus = jihiSbtKingakus;
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
    /// 患者番号
    /// </summary>
    public long PtNum { get; set; }
    /// <summary>
    /// 患者氏名
    /// </summary>
    public string PtName { get; set; }
    public string PtKanaName { get; set; }
    /// <summary>
    /// 性別
    /// </summary>
    public int Sex { get; set; }
    /// <summary>
    /// 生年月日
    /// </summary>
    public int BirthDay { get; set; }
    public string PtSex
    {
        get
        {
            string ret = "男";

            if (Sex == 2)
            {
                ret = "女";
            }

            return ret;
        }
    }
    /// <summary>
    /// 郵便番号
    /// </summary>
    public string PostCd { get; set; }
    public string PostCdDsp
    {
        get { return CIUtil.GetDspPostCd(PostCd); }
    }
    /// <summary>
    /// 住所
    /// </summary>
    public string Address1 { get; set; }
    public string Address2 { get; set; }

    public string Address
    {
        get
        {
            return Address1 ?? "" + Address2 ?? "";
        }
    }
    /// <summary>
    /// 電話番号
    /// </summary>
    public string Tel1 { get; set; }
    public string Tel2 { get; set; }
    public string RenrakuTel { get; set; }
    public string Tel
    {
        get
        {
            string ret = "";

            if (!string.IsNullOrEmpty(Tel1))
            {
                ret = Tel1;
            }
            else if (!string.IsNullOrEmpty(Tel2))
            {
                ret = Tel2;
            }
            else if (!string.IsNullOrEmpty(RenrakuTel))
            {
                ret = RenrakuTel;
            }
            return ret;
        }
    }
    /// <summary>
    /// 請求額
    /// </summary>
    public int SeikyuGaku { get; set; }
    /// <summary>
    /// 入金額
    /// </summary>
    public int NyukinGaku { get; set; }
    /// <summary>
    /// 入金調整額
    /// </summary>
    public int NyukinAdjust { get; set; }
    /// <summary>
    /// 未収額
    /// </summary>
    public int Misyu { get; set; }
    /// <summary>
    /// 総医療費
    /// </summary>
    public int TotalIryohi { get; set; }
    /// <summary>
    /// 点数
    /// </summary>
    public int Tensu { get; set; }
    /// <summary>
    /// 患者負担
    /// </summary>
    public int PtFutan { get; set; }
    public int AdjustFutan { get; set; }
    public int AdjustRound { get; set; }
    /// <summary>
    /// 患者負担合計額
    /// </summary>
    public int TotalPtFutan { get; set; }
    /// <summary>
    /// 自費負担額
    /// </summary>
    public int JihiFutan { get; set; }
    /// <summary>
    /// 自費負担（非課税分）
    /// </summary>
    public int JihiFutanFree { get; set; }
    /// <summary>
    /// 自費負担（通常税率外税分）
    /// </summary>
    public int JihiFutanOuttaxNr { get; set; }
    /// <summary>
    /// 自費負担（軽減税率外税分）
    /// </summary>
    public int JihiFutanOuttaxGen { get; set; }
    /// <summary>
    /// 自費負担（通常税率内税分）
    /// </summary>
    public int JihiFutanTaxNr { get; set; }
    /// <summary>
    /// 自費負担（軽減税率内税分）
    /// </summary>
    public int JihiFutanTaxGen { get; set; }
    /// <summary>
    /// 自費外税
    /// </summary>
    public int JihiOuttax { get; set; }
    /// <summary>
    /// 自費外税（通常税率分）
    /// </summary>
    public int JihiOuttaxNr { get; set; }
    /// <summary>
    /// 自費外税（軽減税率分）
    /// </summary>
    public int JihiOuttaxGen { get; set; }
    /// <summary>
    /// 自費内税
    /// </summary>
    public int JihiTax { get; set; }
    /// <summary>
    /// 自費内税（通常税率分）
    /// </summary>
    public int JihiTaxNr { get; set; }
    /// <summary>
    /// 自費内税（軽減税率分）
    /// </summary>
    public int JihiTaxGen { get; set; }
    /// <summary>
    /// 自費項目金額
    /// </summary>
    public double JihiKoumoku { get; set; }
    /// <summary>
    /// 自費診療金額
    /// </summary>
    public int JihiSinryo { get; set; }
    /// <summary>
    /// 自費種別別金額リスト
    /// </summary>
    public List<(int santeiKbn, int jihiSbt, double kingaku)> JihiKoumokuDtls { get; set; }
    /// <summary>
    /// 指定の自費種別の金額合計を取得する
    /// </summary>
    /// <param name="jihiSbt"></param>
    /// <returns></returns>
    public double JihiKoumokuDtlKingaku(int jihiSbt)
    {
        return JihiKoumokuDtls.Where(p => p.jihiSbt == jihiSbt).Sum(p => p.kingaku);
    }
    /// <summary>
    /// 患者メモ
    /// </summary>
    public CoPtMemoModel PtMemo { get; set; }

    public string Memo
    {
        get
        {
            return PtMemo == null ? "" : PtMemo.Memo ?? "";
        }
    }
    /// <summary>
    /// 患者グループ情報
    /// </summary>
    public List<CoPtGrpInfModel> PtGroupInfModels { get; } 

    /// <summary>
    /// グループコードを取得する
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string PtGroupInfCode(int index)
    {
        string ret = "";

        if (PtGroupInfModels != null && PtGroupInfModels.Any(p => p.GroupId == index))
        {
            ret = PtGroupInfModels?.Find(p => p.GroupId == index)?.GroupCode ?? string.Empty;
        }

        return ret;
    }
    /// <summary>
    /// グループコード名称を取得する
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string PtGroupInfCodeName(int index)
    {
        string ret = "";

        if (PtGroupInfModels != null && PtGroupInfModels.Any(p => p.GroupId == index))
        {
            ret = PtGroupInfModels?.Find(p => p.GroupId == index)?.GrpCdName ?? string.Empty;
        }

        return ret;
    }
    public List<TaxSum> TaxSums { get; } 
    /// <summary>
    /// 自費種別ごとの金額
    /// </summary>
    public List<CoJihiSbtKingakuModel> JihiSbtKingakus { get; } 
}
