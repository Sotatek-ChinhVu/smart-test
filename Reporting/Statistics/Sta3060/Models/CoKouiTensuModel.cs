using Helper.Common;

namespace Reporting.Statistics.Sta3060.Models;

public class CoKouiTensuModel
{
    /// <summary>
    /// 集計区分
    /// </summary>
    public int ReportKbn { get; set; }

    /// <summary>
    /// 集計区分毎にグループ化する値
    /// </summary>
    public long ReportKbnValue
    {
        get
        {
            switch (ReportKbn)
            {
                case 0: //診療日別
                    return SinDate;
                case 1: //診療年月別
                    return SinYm;
                case 2: //診療科別
                    return KaId;
                case 3: //担当医別
                    return TantoId;
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

                    int group3 =
                        new int[] { 2, 5 }.Contains(group1) ? 0 :
                        isPreSchool() ? 2 :  //６未
                        isElder() ? 4 :      //高齢
                        HonkeKbn == 1 ? 1 :  //本人
                        3;                   //家族

                    if (new int[] { 1, 2 }.Contains(HokenKbn))
                    {
                        return group1 * 100 + group2 * 10 + group3;
                    }
                    return group1 * 100;
                case 5: //年齢区分別
                    int age = CIUtil.SDateToAge(Birthday, SinDate);

                    if (age < 1) return 0;
                    if (age < 2) return 1;
                    if (age < 3) return 2;
                    if (age < 6) return 3;
                    if (age < 10) return 4;
                    if (age < 15) return 5;
                    if (age < 20) return 6;
                    if (age < 30) return 7;
                    if (age < 40) return 8;
                    if (age < 50) return 9;
                    if (age < 60) return 10;
                    if (age < 70) return 11;
                    if (age < 80) return 12;
                    if (age < 85) return 13;
                    if (age >= 85) return 14;
                    return -1;
                case 6:  //患者番号
                    return PtNum;
            }

            return 0;
        }
    }

    /// <summary>
    /// 患者ID
    /// </summary>
    public long PtId { get; set; }

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
    public string KaSname { get; set; }

    /// <summary>
    /// 担当医ID
    /// </summary>
    public int TantoId { get; set; }

    /// <summary>
    /// 担当医略称
    /// </summary>
    public string TantoSname { get; set; }

    /// <summary>
    /// 初再診区分
    /// </summary>
    public int SyosaisinKbn { get; set; }

    /// <summary>
    /// 生年月日
    /// </summary>
    public int Birthday { get; set; }

    /// <summary>
    /// ６歳未満未就学児
    /// </summary>
    /// <returns></returns>
    private bool isPreSchool()
    {
        return !CIUtil.IsStudent(Birthday, SinDate);
    }

    /// <summary>
    /// 70歳以上
    /// </summary>
    /// <returns></returns>
    private bool isElder()
    {
        return CIUtil.AgeChk(Birthday, SinDate, 70);
    }

    /// <summary>
    /// 主保険 保険ID
    /// </summary>
    public int HokenId { get; set; }

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
    public string Houbetu { get; set; }

    /// <summary>
    /// 公費件数
    /// </summary>
    public int KohiCount { get; set; }

    /// <summary>
    /// 合計点数
    /// </summary>
    public double TotalTensu { get; set; }

    /// <summary>
    /// 初再診
    /// </summary>
    public double TensuA { get; set; }

    /// <summary>
    /// 医学管理
    /// </summary>
    public double TensuB { get; set; }

    /// <summary>
    /// 在宅
    /// </summary>
    public double TensuC { get; set; }

    /// <summary>
    /// 検査
    /// </summary>
    public double TensuD { get; set; }

    /// <summary>
    /// 画像診断
    /// </summary>
    public double TensuE { get; set; }

    /// <summary>
    /// 投薬
    /// </summary>
    public double TensuF { get; set; }

    /// <summary>
    /// 注射
    /// </summary>
    public double TensuG { get; set; }

    /// <summary>
    /// リハビリ
    /// </summary>
    public double TensuH { get; set; }

    /// <summary>
    /// 精神
    /// </summary>
    public double TensuI { get; set; }

    /// <summary>
    /// 処置
    /// </summary>
    public double TensuJ { get; set; }

    /// <summary>
    /// 手術
    /// </summary>
    public double TensuK { get; set; }

    /// <summary>
    /// 麻酔
    /// </summary>
    public double TensuL { get; set; }

    /// <summary>
    /// 放射線治療
    /// </summary>
    public double TensuM { get; set; }

    /// <summary>
    /// 病理
    /// </summary>
    public double TensuN { get; set; }

    /// <summary>
    /// その他（労災・自賠文書料など）
    /// </summary>
    public double TensuElse
    {
        get => TotalTensu -
            TensuA - TensuB - TensuC - TensuD - TensuE - TensuF - TensuG -
            TensuH - TensuI - TensuJ - TensuK - TensuL - TensuM - TensuN;
    }

    /// <summary>
    /// 自費
    /// </summary>
    public double Jihi { get; set; }

    /// <summary>
    /// 診察
    /// </summary>
    public double Tensu1 { get; set; }

    /// <summary>
    /// 投薬
    /// </summary>
    public double Tensu2 { get; set; }

    /// <summary>
    /// 注射
    /// </summary>
    public double Tensu3 { get; set; }

    /// <summary>
    /// 処置
    /// </summary>
    public double Tensu4 { get; set; }

    /// <summary>
    /// 手術
    /// </summary>
    public double Tensu5 { get; set; }

    /// <summary>
    /// 検査
    /// </summary>
    public double Tensu6 { get; set; }

    /// <summary>
    /// 画像
    /// </summary>
    public double Tensu7 { get; set; }

    /// <summary>
    /// その他
    /// </summary>
    public double Tensu8 { get; set; }

    /// <summary>
    /// 患者負担合計
    /// </summary>
    public int TotalPtFutan { get; set; }

    /// <summary>
    /// 患者番号
    /// </summary>
    public long PtNum { get; set; }

    /// <summary>
    /// 患者氏名
    /// </summary>
    public string PtName { get; set; }

    /// <summary>
    /// 初診(TensuA の内、初診のみ)
    /// </summary>
    public double TensuSyosin { get; set; }

    /// <summary>
    /// 再診(TensuA の内、再診のみ)
    /// </summary>
    public double TensuSaisin { get; set; }

    /// <summary>
    /// 初再診他(TensuA の内、初診・再診以外)
    /// </summary>
    public double TensuSyosaiSonota
    {
        get => TensuA - TensuSyosin - TensuSaisin;
    }
}
