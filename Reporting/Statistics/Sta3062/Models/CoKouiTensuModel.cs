namespace Reporting.Statistics.Sta3062.Models;

public class CoKouiTensuModel
{
    /// <summary>
    /// 集計区分(診療日)
    /// </summary>
    public int ReportKbn { get; set; }

    /// <summary>
    /// 集計区分毎にグループ化する値
    /// </summary>
    public long ReportKbnValue
    {
        get
        {
            //診療日別
            return SinDate;
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
    /// 初再診区分
    /// </summary>
    public int SyosaisinKbn { get; set; }

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
    /// 初診
    /// </summary>
    public double Tensu0 { get; set; }

    /// <summary>
    /// 再診
    /// </summary>
    public double Tensu1 { get; set; }

    /// <summary>
    /// 医学管理
    /// </summary>
    public double Tensu2 { get; set; }

    /// <summary>
    /// 在宅
    /// </summary>
    public double Tensu3 { get; set; }

    /// <summary>
    /// 薬剤器材
    /// </summary>
    public double Tensu4 { get; set; }

    /// <summary>
    /// 投薬
    /// </summary>
    public double Tensu5 { get; set; }

    /// <summary>
    /// 注射
    /// </summary>
    public double Tensu6 { get; set; }

    /// <summary>
    /// 処置
    /// </summary>
    public double Tensu7 { get; set; }

    /// <summary>
    /// 手術
    /// </summary>
    public double Tensu8 { get; set; }

    /// <summary>
    /// 検査
    /// </summary>
    public double Tensu9 { get; set; }

    /// <summary>
    /// 画像
    /// </summary>
    public double Tensu10 { get; set; }

    /// <summary>
    /// その他
    /// </summary>
    public double Tensu11 { get; set; }

    /// <summary>
    /// 自費
    /// </summary>
    public double Jihi { get; set; }

    /// <summary>
    /// 医療費合計
    /// </summary>
    public int TotalIryohi { get; set; }
}
