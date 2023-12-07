using Entity.Tenant;
using Helper.Common;
using Helper.Extension;

namespace Reporting.Statistics.Sta9000.Models;

public class CoPtHokenModel
{
    public PtHokenPattern PtHokenPattern { get; private set; }
    public PtHokenInf PtHokenInf { get; private set; }
    public PtKohi PtKohi1 { get; private set; }
    public PtKohi PtKohi2 { get; private set; }
    public PtKohi PtKohi3 { get; private set; }
    public PtKohi PtKohi4 { get; private set; }

    public CoPtHokenModel(PtHokenPattern ptHokenPattern, PtHokenInf ptHokenInf,
        PtKohi ptKohi1, PtKohi ptKohi2, PtKohi ptKohi3, PtKohi ptKohi4)
    {
        PtHokenPattern = ptHokenPattern;
        PtHokenInf = ptHokenInf;
        PtKohi1 = ptKohi1;
        PtKohi2 = ptKohi2;
        PtKohi3 = ptKohi3;
        PtKohi4 = ptKohi4;
    }

    public CoPtHokenModel()
    {
        PtHokenPattern = new();
        PtHokenInf = new();
        PtKohi1 = new();
        PtKohi2 = new();
        PtKohi3 = new();
        PtKohi4 = new();
    }

    /// <summary>
    /// 患者ID
    /// </summary>
    public long PtId
    {
        get => PtHokenPattern.PtId;
    }

    /// <summary>
    /// 保険組合せID
    /// </summary>
    public int HokenPid
    {
        get => PtHokenPattern.HokenPid;
    }

    /// <summary>
    /// 保険区分
    /// </summary>
    public int HokenKbn
    {
        get => PtHokenPattern.HokenKbn;
    }

    /// <summary>
    /// 保険区分名称
    /// </summary>
    public string HokenKbnName
    {
        get
        {
            switch (PtHokenPattern.HokenKbn)
            {
                case 0: return "自費";
                case 1: return "社保";
                case 2: return "国保";
                case 11: return "労災(短期給付)";
                case 12: return "労災(傷病年金)";
                case 13: return "アフターケア";
                case 14: return "自賠責";
            }
            return "";
        }
    }

    /// <summary>
    /// 保険種別コード
    /// </summary>
    public int HokenSbtCd
    {
        get => PtHokenPattern.HokenSbtCd;
    }

    /// <summary>
    /// 保険種別コード名称
    /// </summary>
    public string HokenSbtCdName
    {
        get
        {
            if (PtHokenPattern.HokenSbtCd == 0) return string.Empty;

            string hokenSbt = string.Empty;
            switch (PtHokenPattern.HokenSbtCd / 100)
            {
                case 1: hokenSbt = "社保"; break;
                case 2: hokenSbt = "国保"; break;
                case 3: hokenSbt = "後期"; break;
                case 4: hokenSbt = "退職"; break;
                case 5: hokenSbt = "公費"; break;
            }

            if (hokenSbt != string.Empty)
            {
                switch (HokenSbtCd % 10)
                {
                    case 1: hokenSbt += "単独"; break;
                    case 2: hokenSbt += "２併"; break;
                    case 3: hokenSbt += "３併"; break;
                    case 4: hokenSbt += "４併"; break;
                    case 5: hokenSbt += "５併"; break;
                }
            }

            return hokenSbt;
        }
    }

    /// <summary>
    /// 保険メモ
    /// </summary>
    public string HokenMemo
    {
        get => PtHokenPattern?.HokenMemo?.AsString().Replace(Environment.NewLine, "⏎") ?? string.Empty;
    }

    /// <summary>
    /// 適用開始日
    /// </summary>
    public string StartDate
    {
        get => CIUtil.SDateToShowSDate(PtHokenPattern.StartDate);
    }

    /// <summary>
    /// 適用終了日
    /// </summary>
    public string EndDate
    {
        get => CIUtil.SDateToShowSDate(PtHokenPattern.EndDate);
    }

    /// <summary>
    /// 主保険 保険ID
    /// </summary>
    public int HokenId
    {
        get => PtHokenPattern.HokenId;
    }

    public int HokenNo
    {
        get => PtHokenInf.HokenNo;
    }

    public int HokenEdaNo
    {
        get => PtHokenInf.HokenEdaNo;
    }

    public string HokensyaNo
    {
        get => PtHokenInf.HokensyaNo ?? string.Empty;
    }

    public string Kigo
    {
        get => PtHokenInf.Kigo ?? string.Empty;
    }

    public string Bango
    {
        get => PtHokenInf.Bango ?? string.Empty;
    }

    public string EdaNo
    {
        get => PtHokenInf.EdaNo ?? string.Empty;
    }

    public int HonkeKbn
    {
        get => PtHokenInf.HonkeKbn;
    }

    public string Houbetu
    {
        get => PtHokenInf.Houbetu ?? string.Empty;
    }

    public string HokensyaName
    {
        get => PtHokenInf.HokensyaName ?? string.Empty;
    }

    public string HokensyaPost
    {
        get => PtHokenInf.HokensyaPost ?? string.Empty;
    }

    public string HokensyaAddress
    {
        get => PtHokenInf.HokensyaAddress ?? string.Empty;
    }

    public string HokensyaTel
    {
        get => PtHokenInf.HokensyaTel ?? string.Empty;
    }

    public int KeizokuKbn
    {
        get => PtHokenInf.KeizokuKbn;
    }

    public string SikakuDate
    {
        get => CIUtil.SDateToShowSDate(PtHokenInf.SikakuDate);
    }

    public string KofuDate
    {
        get => CIUtil.SDateToShowSDate(PtHokenInf.KofuDate);
    }

    public int KogakuKbn
    {
        get => PtHokenInf.KogakuKbn;
    }

    public int KogakuType
    {
        get => PtHokenInf.KogakuType;
    }

    public int TokureiYm1
    {
        get => PtHokenInf.TokureiYm1;
    }

    public int TokureiYm2
    {
        get => PtHokenInf.TokureiYm2;
    }

    public int TasukaiYm
    {
        get => PtHokenInf.TasukaiYm;
    }

    public int SyokumuKbn
    {
        get => PtHokenInf.SyokumuKbn;
    }

    public int GenmenKbn
    {
        get => PtHokenInf.GenmenKbn;
    }

    public int GenmenRate
    {
        get => PtHokenInf.GenmenRate;
    }

    public int GenmenGaku
    {
        get => PtHokenInf.GenmenGaku;
    }

    public string Tokki1
    {
        get => PtHokenInf.Tokki1 ?? string.Empty;
    }

    public string Tokki2
    {
        get => PtHokenInf.Tokki2 ?? string.Empty;
    }

    public string Tokki3
    {
        get => PtHokenInf.Tokki3 ?? string.Empty;
    }

    public string Tokki4
    {
        get => PtHokenInf.Tokki4 ?? string.Empty;
    }

    public string Tokki5
    {
        get => PtHokenInf.Tokki5 ?? string.Empty;
    }

    public string RousaiKofuNo
    {
        get => PtHokenInf.RousaiKofuNo ?? string.Empty;
    }

    public int RousaiSaigaiKbn
    {
        get => PtHokenInf.RousaiSaigaiKbn;
    }

    public string RousaiJigyosyoName
    {
        get => PtHokenInf.RousaiJigyosyoName ?? string.Empty;
    }

    public string RousaiPrefName
    {
        get => PtHokenInf.RousaiPrefName ?? string.Empty;
    }

    public string RousaiCityName
    {
        get => PtHokenInf.RousaiCityName ?? string.Empty;
    }

    public string RousaiSyobyoDate
    {
        get => CIUtil.SDateToShowSDate(PtHokenInf.RousaiSyobyoDate);
    }

    public string RousaiSyobyoCd
    {
        get => PtHokenInf.RousaiSyobyoCd ?? string.Empty;
    }

    public string RousaiRoudouCd
    {
        get => PtHokenInf.RousaiRoudouCd ?? string.Empty;
    }

    public string RousaiKantokuCd
    {
        get => PtHokenInf.RousaiKantokuCd ?? string.Empty;
    }

    public int RousaiReceCount
    {
        get => PtHokenInf.RousaiReceCount;
    }

    public string RyoyoStartDate
    {
        get => CIUtil.SDateToShowSDate(PtHokenInf.RyoyoStartDate);
    }

    public string RyoyoEndDate
    {
        get => CIUtil.SDateToShowSDate(PtHokenInf.RyoyoEndDate);
    }

    public string JibaiHokenName
    {
        get => PtHokenInf.JibaiHokenName ?? string.Empty;
    }

    public string JibaiHokenTanto
    {
        get => PtHokenInf.JibaiHokenTanto ?? string.Empty;
    }

    public string JibaiHokenTel
    {
        get => PtHokenInf.JibaiHokenTel ?? string.Empty;
    }

    public string JibaiJyusyouDate
    {
        get => CIUtil.SDateToShowSDate(PtHokenInf.JibaiJyusyouDate);
    }

    #region 公費１
    public int Kohi1Id
    {
        get => PtHokenPattern.Kohi1Id;
    }

    public int Kohi1PrefNo
    {
        get => PtKohi1?.PrefNo ?? 0;
    }

    public int Kohi1HokenNo
    {
        get => PtKohi1?.HokenNo ?? 0;
    }

    public int Kohi1HokenEdaNo
    {
        get => PtKohi1?.HokenEdaNo ?? 0;
    }

    public string Kohi1FutansyaNo
    {
        get => PtKohi1?.FutansyaNo ?? string.Empty;
    }

    public string Kohi1JyukyusyaNo
    {
        get => PtKohi1?.JyukyusyaNo ?? string.Empty;
    }

    public string Kohi1TokusyuNo
    {
        get => PtKohi1?.TokusyuNo ?? string.Empty;
    }

    public int Kohi1SikakuDate
    {
        get => PtKohi1?.SikakuDate ?? 0;
    }

    public int Kohi1KofuDate
    {
        get => PtKohi1?.KofuDate ?? 0;
    }

    public int Kohi1StartDate
    {
        get => PtKohi1?.StartDate ?? 0;
    }

    public int Kohi1EndDate
    {
        get => PtKohi1?.EndDate ?? 0;
    }

    public int Kohi1Rate
    {
        get => PtKohi1?.Rate ?? 0;
    }

    public int Kohi1GendoGaku
    {
        get => PtKohi1?.GendoGaku ?? 0;
    }
    #endregion

    #region 公費２
    public int Kohi2Id
    {
        get => PtHokenPattern.Kohi2Id;
    }

    public int Kohi2PrefNo
    {
        get => PtKohi2?.PrefNo ?? 0;
    }

    public int Kohi2HokenNo
    {
        get => PtKohi2?.HokenNo ?? 0;
    }

    public int Kohi2HokenEdaNo
    {
        get => PtKohi2?.HokenEdaNo ?? 0;
    }

    public string Kohi2FutansyaNo
    {
        get => PtKohi2?.FutansyaNo ?? string.Empty;
    }

    public string Kohi2JyukyusyaNo
    {
        get => PtKohi2?.JyukyusyaNo ?? string.Empty;
    }

    public string Kohi2TokusyuNo
    {
        get => PtKohi2?.TokusyuNo ?? string.Empty;
    }

    public int Kohi2SikakuDate
    {
        get => PtKohi2?.SikakuDate ?? 0;
    }

    public int Kohi2KofuDate
    {
        get => PtKohi2?.KofuDate ?? 0;
    }

    public int Kohi2StartDate
    {
        get => PtKohi2?.StartDate ?? 0;
    }

    public int Kohi2EndDate
    {
        get => PtKohi2?.EndDate ?? 0;
    }

    public int Kohi2Rate
    {
        get => PtKohi2?.Rate ?? 0;
    }

    public int Kohi2GendoGaku
    {
        get => PtKohi2?.GendoGaku ?? 0;
    }
    #endregion

    #region 公費３
    public int Kohi3Id
    {
        get => PtHokenPattern.Kohi3Id;
    }

    public int Kohi3PrefNo
    {
        get => PtKohi3?.PrefNo ?? 0;
    }

    public int Kohi3HokenNo
    {
        get => PtKohi3?.HokenNo ?? 0;
    }

    public int Kohi3HokenEdaNo
    {
        get => PtKohi3?.HokenEdaNo ?? 0;
    }

    public string Kohi3FutansyaNo
    {
        get => PtKohi3?.FutansyaNo ?? string.Empty;
    }

    public string Kohi3JyukyusyaNo
    {
        get => PtKohi3?.JyukyusyaNo ?? string.Empty;
    }

    public string Kohi3TokusyuNo
    {
        get => PtKohi3?.TokusyuNo ?? string.Empty;
    }

    public int Kohi3SikakuDate
    {
        get => PtKohi3?.SikakuDate ?? 0;
    }

    public int Kohi3KofuDate
    {
        get => PtKohi3?.KofuDate ?? 0;
    }

    public int Kohi3StartDate
    {
        get => PtKohi3?.StartDate ?? 0;
    }

    public int Kohi3EndDate
    {
        get => PtKohi3?.EndDate ?? 0;
    }

    public int Kohi3Rate
    {
        get => PtKohi3?.Rate ?? 0;
    }

    public int Kohi3GendoGaku
    {
        get => PtKohi3?.GendoGaku ?? 0;
    }
    #endregion

    #region 公費４
    public int Kohi4Id
    {
        get => PtHokenPattern.Kohi4Id;
    }

    public int Kohi4PrefNo
    {
        get => PtKohi4?.PrefNo ?? 0;
    }

    public int Kohi4HokenNo
    {
        get => PtKohi4?.HokenNo ?? 0;
    }

    public int Kohi4HokenEdaNo
    {
        get => PtKohi4?.HokenEdaNo ?? 0;
    }

    public string Kohi4FutansyaNo
    {
        get => PtKohi4?.FutansyaNo ?? string.Empty;
    }

    public string Kohi4JyukyusyaNo
    {
        get => PtKohi4?.JyukyusyaNo ?? string.Empty;
    }

    public string Kohi4TokusyuNo
    {
        get => PtKohi4?.TokusyuNo ?? string.Empty;
    }

    public int Kohi4SikakuDate
    {
        get => PtKohi4?.SikakuDate ?? 0;
    }

    public int Kohi4KofuDate
    {
        get => PtKohi4?.KofuDate ?? 0;
    }

    public int Kohi4StartDate
    {
        get => PtKohi4?.StartDate ?? 0;
    }

    public int Kohi4EndDate
    {
        get => PtKohi4?.EndDate ?? 0;
    }

    public int Kohi4Rate
    {
        get => PtKohi4?.Rate ?? 0;
    }

    public int Kohi4GendoGaku
    {
        get => PtKohi4?.GendoGaku ?? 0;
    }
    #endregion
}
