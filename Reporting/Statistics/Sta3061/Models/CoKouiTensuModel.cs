using Helper.Common;

namespace Reporting.Statistics.Sta3061.Models;

public class CoKouiTensuModel
{
    /// <summary>
    /// 集計区分
    /// </summary>
    public int ReportKbn { get; set; }

    /// <summary>
    /// 集計区分毎にグループ化する値
    /// </summary>
    public string ReportKbnValue
    {
        get
        {
            switch (ReportKbn)
            {
                case 0: //診療日別
                    return SinDate.ToString();
                case 1: //診療年月別
                    return SinYm.ToString();
                case 2: //診療科別
                    return KaId.ToString();
                case 3: //担当医別
                    return TantoId.ToString();
                case 4: //保険種別
                    int group1 =
                        HokenSbtCd1 == 1 ? 1 :                       //社保
                        HokenSbtCd1 == 5 ? 2 :                       //公費
                        HokenSbtCd1 == 2 ? 3 :                       //国保
                        HokenSbtCd1 == 4 ? 4 :                       //退職
                        HokenSbtCd1 == 3 ? 5 :                       //後期
                        new int[] { 11, 12, 13 }.Contains(HokenKbn) ? 8 : //労災
                        HokenKbn == 14 ? 9 :                              //自賠
                        HokenKbn == 0 && Houbetu != "108" ? 7 :           //自レ
                        6;                                                //自費

                    int group2 =
                        group1 == 2 && KohiCount >= 2 ? 2 :   //併用
                        group1 != 2 && KohiCount >= 1 ? 2 :   //併用
                        1;                                    //単独

                    if (new int[] { 1, 2 }.Contains(HokenKbn))
                    {
                        return (group1 * 10 + group2).ToString();
                    }
                    return (group1 * 10).ToString();
                case 5: //年齢区分別
                    int age = CIUtil.SDateToAge(Birthday, SinDate);

                    if (age < 1) return "0";
                    if (age < 2) return "1";
                    if (age < 3) return "2";
                    if (age < 6) return "3";
                    if (age < 10) return "4";
                    if (age < 15) return "5";
                    if (age < 20) return "6";
                    if (age < 30) return "7";
                    if (age < 40) return "8";
                    if (age < 50) return "9";
                    if (age < 60) return "10";
                    if (age < 70) return "11";
                    if (age < 80) return "12";
                    if (age < 85) return "13";
                    if (age >= 85) return "14";
                    return "-1";
                case 6: //性別
                    return Sex.ToString();
            }
            //患者グループ
            if (PtGrpId > 0)
            {
                return PtGrpCode;
            }

            return "0";
        }
    }

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
    public string PtName { get; set; } = string.Empty;

    /// <summary>
    /// 性別
    /// </summary>
    public int Sex { get; set; }

    /// <summary>
    /// 患者グループID
    /// </summary>
    public int PtGrpId
    {
        get => ReportKbn > 10 ? ReportKbn - 10 : 0;
    }

    /// <summary>
    /// 来院番号
    /// </summary>
    public long RaiinNo { get; set; }

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
    /// 生年月日
    /// </summary>
    public int Birthday { get; set; }

    /// <summary>
    /// 保険区分
    ///		0:自費
    ///		1:社保
    ///		2:国保
    ///		11:労災(短期給付)
    ///		12:労災(傷病年金)
    ///		13:アフターケア
    ///		14:自賠責
    /// </summary>
    public int HokenKbn { get; set; }

    /// <summary>
    /// 保険種別コードの1桁目
    ///     1:社保 2:国保 3:後期 4:退職 5:公費          
    /// </summary>
    public int HokenSbtCd1 { get; set; }

    /// <summary>
    /// 本人家族区分
    ///		1:本人
    ///		2:家族
    /// </summary>
    public int HonkeKbn { get; set; }

    /// <summary>
    /// 法別番号
    /// </summary>
    public string Houbetu { get; set; } = string.Empty;

    /// <summary>
    /// 公費件数
    /// </summary>
    public int KohiCount { get; set; }

    /// <summary>
    /// 合計点数
    /// </summary>
    public double TotalTensu { get; set; }

    /// <summary>
    /// 診察計
    /// </summary>
    public double Tensu1x { get; set; }

    /// <summary>
    /// 初診
    /// </summary>
    public double Tensu11 { get; set; }

    /// <summary>
    /// 再診
    /// </summary>
    public double Tensu12 { get; set; }

    /// <summary>
    /// 医学管理
    /// </summary>
    public double Tensu13 { get; set; }

    /// <summary>
    /// 在宅
    /// </summary>
    public double Tensu14x { get; set; }

    /// <summary>
    /// 在宅薬剤
    /// </summary>
    public double Tensu1450 { get; set; }

    /// <summary>
    /// 投薬計
    /// </summary>
    public double Tensu2x { get; set; }

    /// <summary>
    /// 薬剤
    /// </summary>
    public double Tensu2100 { get; set; }

    /// <summary>
    /// 調剤
    /// </summary>
    public double Tensu2110 { get; set; }

    /// <summary>
    /// 処方
    /// </summary>
    public double Tensu2500 { get; set; }

    /// <summary>
    /// 麻毒
    /// </summary>
    public double Tensu2600 { get; set; }

    /// <summary>
    /// 調基
    /// </summary>
    public double Tensu2700 { get; set; }

    /// <summary>
    /// 注射計
    /// </summary>
    public double Tensu3x { get; set; }

    /// <summary>
    /// 皮下筋肉内注射
    /// </summary>
    public double Tensu31 { get; set; }

    /// <summary>
    /// 静脈内注射
    /// </summary>
    public double Tensu32 { get; set; }

    /// <summary>
    /// その他注射
    /// </summary>
    public double Tensu33 { get; set; }

    /// <summary>
    /// 注射(薬剤器材)
    /// </summary>
    public double Tensu39 { get; set; }

    /// <summary>
    /// 処置計
    /// </summary>
    public double Tensu4x { get; set; }

    /// <summary>
    /// 処置手技
    /// </summary>
    public double Tensu4000 { get; set; }

    /// <summary>
    /// 処置薬剤
    /// </summary>
    public double Tensu4010 { get; set; }

    /// <summary>
    /// 手術計
    /// </summary>
    public double Tensu5x { get; set; }

    /// <summary>
    /// 手術手技
    /// </summary>
    public double Tensu5000 { get; set; }

    /// <summary>
    /// 手術薬剤
    /// </summary>
    public double Tensu5010 { get; set; }

    /// <summary>
    /// 検査計
    /// </summary>
    public double Tensu6x { get; set; }

    /// <summary>
    /// 検査手技
    /// </summary>
    public double Tensu6000 { get; set; }

    /// <summary>
    /// 検査薬剤
    /// </summary>
    public double Tensu6010 { get; set; }

    /// <summary>
    /// 画像計
    /// </summary>
    public double Tensu7x { get; set; }

    /// <summary>
    /// 画像手技
    /// </summary>
    public double Tensu7000 { get; set; }

    /// <summary>
    /// 画像薬剤
    /// </summary>
    public double Tensu7010 { get; set; }

    /// <summary>
    /// 画像フィルム
    /// </summary>
    public double Tensu7f { get; set; }

    /// <summary>
    /// その他計
    /// </summary>
    public double Tensu8x { get; set; }

    /// <summary>
    /// その他(処方せん)
    /// </summary>
    public double Tensu8000 { get; set; }

    /// <summary>
    /// その他(その他)
    /// </summary>
    public double Tensu8010 { get; set; }

    /// <summary>
    /// その他(薬剤)
    /// </summary>
    public double Tensu8020 { get; set; }

    /// <summary>
    /// 自費計
    /// </summary>
    public double Jihi { get; set; }

    /// <summary>
    /// 自費明細
    /// </summary>
    public List<double> JihiMeisais { get; set; } = new();

    /// <summary>
    /// 診察計
    /// </summary>
    public int Count1x { get; set; }

    /// <summary>
    /// 初診
    /// </summary>
    public int Count11 { get; set; }

    /// <summary>
    /// 再診
    /// </summary>
    public int Count12 { get; set; }

    /// <summary>
    /// 医学管理
    /// </summary>
    public int Count13 { get; set; }

    /// <summary>
    /// 在宅
    /// </summary>
    public int Count14x { get; set; }

    /// <summary>
    /// 在宅薬剤
    /// </summary>
    public int Count1450 { get; set; }

    /// <summary>
    /// 投薬計
    /// </summary>
    public int Count2x { get; set; }

    /// <summary>
    /// 薬剤
    /// </summary>
    public int Count2100 { get; set; }

    /// <summary>
    /// 調剤
    /// </summary>
    public int Count2110 { get; set; }

    /// <summary>
    /// 処方
    /// </summary>
    public int Count2500 { get; set; }

    /// <summary>
    /// 麻毒
    /// </summary>
    public int Count2600 { get; set; }

    /// <summary>
    /// 調基
    /// </summary>
    public int Count2700 { get; set; }

    /// <summary>
    /// 注射計
    /// </summary>
    public int Count3x { get; set; }

    /// <summary>
    /// 皮下筋肉内注射
    /// </summary>
    public int Count31 { get; set; }

    /// <summary>
    /// 静脈内注射
    /// </summary>
    public int Count32 { get; set; }

    /// <summary>
    /// その他注射
    /// </summary>
    public int Count33 { get; set; }

    /// <summary>
    /// 注射(薬剤器材)
    /// </summary>
    public int Count39 { get; set; }

    /// <summary>
    /// 処置計
    /// </summary>
    public int Count4x { get; set; }

    /// <summary>
    /// 処置手技
    /// </summary>
    public int Count4000 { get; set; }

    /// <summary>
    /// 処置薬剤
    /// </summary>
    public int Count4010 { get; set; }

    /// <summary>
    /// 手術計
    /// </summary>
    public int Count5x { get; set; }

    /// <summary>
    /// 手術手技
    /// </summary>
    public int Count5000 { get; set; }

    /// <summary>
    /// 手術薬剤
    /// </summary>
    public int Count5010 { get; set; }

    /// <summary>
    /// 検査計
    /// </summary>
    public int Count6x { get; set; }

    /// <summary>
    /// 検査手技
    /// </summary>
    public int Count6000 { get; set; }

    /// <summary>
    /// 検査薬剤
    /// </summary>
    public int Count6010 { get; set; }

    /// <summary>
    /// 画像計
    /// </summary>
    public int Count7x { get; set; }

    /// <summary>
    /// 画像手技
    /// </summary>
    public int Count7000 { get; set; }

    /// <summary>
    /// 画像薬剤
    /// </summary>
    public int Count7010 { get; set; }

    /// <summary>
    /// 画像フィルム
    /// </summary>
    public int Count7f { get; set; }

    /// <summary>
    /// その他計
    /// </summary>
    public int Count8x { get; set; }

    /// <summary>
    /// その他(処方せん)
    /// </summary>
    public int Count8000 { get; set; }

    /// <summary>
    /// その他(その他)
    /// </summary>
    public int Count8010 { get; set; }

    /// <summary>
    /// その他(薬剤)
    /// </summary>
    public int Count8020 { get; set; }

    /// <summary>
    /// 自費計
    /// </summary>
    public int CountJihi { get; set; }

    /// <summary>
    /// 自費明細
    /// </summary>
    public List<int> CountJihiMeisais { get; set; } = new();

    /// <summary>
    /// 患者グループコード
    /// </summary>
    public string PtGrpCode { get; set; } = string.Empty;

    /// <summary>
    /// 患者グループコード名
    /// </summary>
    public string PtGrpCodeName { get; set; } = string.Empty;
}
