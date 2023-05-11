using Helper.Common;

namespace Reporting.Statistics.Sta3071.Models;

public class CoRaiinInfModel
{
    /// <summary>
    /// 病院コード
    /// </summary>
    public int HpId { get; set; }

    /// <summary>
    /// 診療日
    /// </summary>
    public int SinDate { get; set; }

    /// <summary>
    /// 診療年月
    /// </summary>
    public int SinYm
    {
        get => SinDate / 100;
    }

    /// <summary>
    /// 来院番号
    /// </summary>
    public long RaiinNo { get; set; }

    /// <summary>
    /// 患者番号
    /// </summary>
    public long PtId { get; set; }

    /// <summary>
    /// 性別
    /// </summary>
    public int Sex { get; set; }

    /// <summary>
    /// 誕生日
    /// </summary>
    public int BirthDay { get; set; }

    /// <summary>
    /// 年齢
    /// </summary>
    public int Age
    {
        get => CIUtil.SDateToAge(BirthDay, SinDate);
    }

    /// <summary>
    /// 初再診区分
    ///    0:なし 1:初診 2:初診紹介※未使用 3:再診 4:電話再診 6:同日初診 7:再診２科目 8:電話再診２科目
    /// </summary>
    public int SyosaisinKbn { get; set; }

    /// <summary>
    /// 時間枠区分
    ///   0:時間内 1:時間外 2:休日 3:深夜 4:夜・早 5:夜間小特 6:休日小特 7:深夜小特
    /// </summary>
    public int JikanKbn { get; set; }

    /// <summary>
    /// 最初の来院日
    /// </summary>
    public int MinSinDate { get; set; }

    /// <summary>
    /// 新患
    /// </summary>
    public bool IsSinkan
    {
        get => SinDate == MinSinDate;
    }

    /// <summary>
    /// 診療科ID
    /// </summary>
    public int KaId { get; set; }

    /// <summary>
    /// 診療科略称
    /// </summary>
    public string KaSname { get; set; } = string.Empty;

    /// <summary>
    /// 担当医ID
    /// </summary>
    public int TantoId { get; set; }

    /// <summary>
    /// 担当医略称
    /// </summary>
    public string TantoSname { get; set; } = string.Empty;

    /// <summary>
    /// 保険区分
    /// </summary>
    public int HokenKbn { get; set; }

    /// <summary>
    /// 法別番号
    /// </summary>
    public string Houbetu { get; set; } = string.Empty;

    /// <summary>
    /// 保険種別コード
    /// </summary>
    public int HokenSbtCd { get; set; }

    /// <summary>
    /// 集計項目（縦）
    ///     1:診療科別 2:担当医別 3:保険種別 4:日別 5:月別 6:時間外等別 7:年齢区分別別 8:性別 2X:患者グループX
    /// </summary>
    public int ReportKbnV { get; set; }

    /// <summary>
    /// 集計項目（横）
    ///     1:診療科別 2:担当医別 3:保険種別 4:日別 5:月別 6:時間外等別 7:年齢区分別別 8:性別 2X:患者グループX
    /// </summary>
    public int ReportKbnH { get; set; }

    /// <summary>
    /// 患者グループ
    /// </summary>
    public struct PtGrp
    {
        public int GrpId { get; set; }
        public string GrpName { get; set; }
        public string GrpCode { get; set; }
        public string GrpCodeName { get; set; }

        public PtGrp(int grpId, string grpName, string grpCode, string grpCodeName)
        {
            GrpId = grpId;
            GrpName = grpName;
            GrpCode = grpCode;
            GrpCodeName = grpCodeName;
        }
    };

    /// <summary>
    /// 患者グループリスト
    /// </summary>
    public List<PtGrp> PtGrps { get; set; } = new();

    public const int PtGrtStartIdx = 20;

    /// <summary>
    /// 集計項目（縦）毎にグループ化する値
    /// </summary>
    public string ReportKbnVValue
    {
        get => GetReportKbnValue(ReportKbnV);
    }

    /// <summary>
    /// 集計項目（横）毎にグループ化する値
    /// </summary>
    public string ReportKbnHValue
    {
        get => GetReportKbnValue(ReportKbnH);
    }

    private string GetReportKbnValue(int ReportKbn)
    {
        if (ReportKbn > PtGrtStartIdx)
        {
            //患者グループ
            string retstr = PtGrps?.FirstOrDefault(p => p.GrpId == (ReportKbn - PtGrtStartIdx)).GrpCode ?? string.Empty;
            return retstr;
        }
        else
        {
            switch (ReportKbn)
            {
                case 1: //診療科別
                    return KaId.ToString("D4");
                case 2: //担当医別
                    return TantoId.ToString("D4");
                case 3: //保険種別
                    int group1 =
                        HokenSbtCd / 100 == 1 ? 1 :                       //社保
                        HokenSbtCd / 100 == 5 ? 2 :                       //公費
                        HokenSbtCd / 100 == 2 ? 3 :                       //国保
                        HokenSbtCd / 100 == 4 ? 4 :                       //退職
                        HokenSbtCd / 100 == 3 ? 5 :                       //後期
                        new int[] { 11, 12, 13 }.Contains(HokenKbn) ? 8 : //労災
                        HokenKbn == 14 ? 9 :                              //自賠
                        HokenKbn == 0 && Houbetu != "108" ? 7 :           //自レ
                        6;                                                //自費

                    int group2 =
                        HokenSbtCd % 10 >= 2 ? 2 :  //併用
                        1;                          //単独

                    int ret = group1 * 10;
                    if (new int[] { 1, 2 }.Contains(HokenKbn))
                    {
                        ret += group2;
                    }
                    return ret.ToString("D2");
                case 4: //日別
                    return SinDate.ToString("D8");
                case 5: //月別
                    return SinYm.ToString("D6");
                case 6: //時間外等別
                    int rJikan =
                        JikanKbn == 0 ? 0 :                         //時間内
                        JikanKbn == 1 ? 1 :                         //時間外
                        new int[] { 2, 6 }.Contains(JikanKbn) ? 2 : //休日
                        new int[] { 3, 7 }.Contains(JikanKbn) ? 3 : //深夜
                        new int[] { 4, 5 }.Contains(JikanKbn) ? 4 : //夜間早朝
                        -1;
                    return rJikan.ToString();
                case 7: //年齢区分別
                    int age =
                        Age < 1 ? 0 :
                        Age < 2 ? 1 :
                        Age < 3 ? 2 :
                        Age < 6 ? 3 :
                        Age < 10 ? 4 :
                        Age < 15 ? 5 :
                        Age < 20 ? 6 :
                        Age < 30 ? 7 :
                        Age < 40 ? 8 :
                        Age < 50 ? 9 :
                        Age < 60 ? 10 :
                        Age < 70 ? 11 :
                        Age < 80 ? 12 :
                        Age < 85 ? 13 :
                        Age >= 85 ? 14 :
                        -1;
                    return age.ToString("D2");
                case 8: //性別
                    return (Sex == 1 || Sex == 2) ? Sex.ToString() : "0";
            }
        }

        return String.Empty;
    }
}
