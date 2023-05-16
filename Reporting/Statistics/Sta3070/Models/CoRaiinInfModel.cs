using Helper.Common;

namespace Reporting.Statistics.Sta3070.Models;

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
    /// 診療年
    /// </summary>
    public int SinY
    {
        get => SinDate / 10000;
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
    public string? KaSname { get; set; }

    /// <summary>
    /// 担当医ID
    /// </summary>
    public int TantoId { get; set; }

    /// <summary>
    /// 担当医略称
    /// </summary>
    public string? TantoSname { get; set; }

    /// <summary>
    /// 保険区分
    /// </summary>
    public int HokenKbn { get; set; }

    /// <summary>
    /// 法別番号
    /// </summary>
    public string? Houbetu { get; set; }

    /// <summary>
    /// 保険種別コード
    /// </summary>
    public int HokenSbtCd { get; set; }

    /// <summary>
    /// 本家区分
    /// </summary>
    public int HonkeKbn { get; set; }

    /// <summary>
    /// 集計区分
    ///     0:診療年月別 1:診療年別 2:診療科別 3:担当医別 4:保険種別 5:年齢区分別 
    /// </summary>
    public int ReportKbn { get; set; }

    /// <summary>
    /// 集計区分毎にグループ化する値
    /// </summary>
    public int ReportKbnValue
    {
        get
        {
            switch (ReportKbn)
            {
                case 0: //診療年月別
                    return SinYm;
                case 1: //診療年別
                    return SinY;
                case 2: //診療科別
                    return KaId;
                case 3: //担当医別
                    return TantoId;
                case 4: //保険種別
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

                    int group3 =
                        IsPreSchool() ? 2 :            //６未
                        group1 != 4 && IsElder() ? 4 : //高齢
                        HonkeKbn == 1 ? 1 :            //本人
                        3;                             //家族

                    int ret = group1 * 100;
                    if (new int[] { 1, 2 }.Contains(HokenKbn))
                    {
                        ret += group2 * 10;
                        if (new int[] { 1, 3, 4, 5 }.Contains(group1))
                        {
                            ret += group3;
                        }
                    }
                    return ret;
                case 5: //年齢区分別
                    if (Age < 1) return 0;
                    if (Age < 2) return 1;
                    if (Age < 3) return 2;
                    if (Age < 6) return 3;
                    if (Age < 10) return 4;
                    if (Age < 15) return 5;
                    if (Age < 20) return 6;
                    if (Age < 30) return 7;
                    if (Age < 40) return 8;
                    if (Age < 50) return 9;
                    if (Age < 60) return 10;
                    if (Age < 70) return 11;
                    if (Age < 80) return 12;
                    if (Age < 85) return 13;
                    if (Age >= 85) return 14;
                    return -1;
            }

            return 0;
        }
    }

    private bool IsPreSchool()
    {
        return !CIUtil.IsStudent(BirthDay, SinDate);
    }

    private bool IsElder()
    {
        return CIUtil.AgeChk(BirthDay, SinDate, 70);
    }
}