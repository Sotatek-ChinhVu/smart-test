using Reporting.Statistics.Enums;

namespace Reporting.Statistics.Sta3061.Models;

public class CoSta3061PrintData
{
    public CoSta3061PrintData(RowType rowType = RowType.Data)
    {
        RowType = rowType;
    }

    public class CountDetail
    {
        /// <summary>
        /// 回数
        /// </summary>
        public string Count { get; set; } = string.Empty;

        /// <summary>
        /// 点数/金額
        /// </summary>
        public string Tensu { get; set; } = string.Empty;

        /// <summary>
        /// 1来院当り点数/金額
        /// </summary>
        public string RaiinTensu { get; set; } = string.Empty;

        /// <summary>
        /// 構成比％
        /// </summary>
        public string Rate { get; set; } = string.Empty;
    }

    /// <summary>
    /// 行タイプ
    /// </summary>
    public RowType RowType { get; set; }

    /// <summary>
    /// 集計区分
    /// </summary>
    public string ReportKbn { get; set; }

    /// <summary>
    /// 診察計
    /// </summary>
    public CountDetail Tensu1x { get; set; } = new CountDetail();

    /// <summary>
    /// 初診
    /// </summary>
    public CountDetail Tensu11 { get; set; } = new CountDetail();

    /// <summary>
    /// 再診
    /// </summary>
    public CountDetail Tensu12 { get; set; } = new CountDetail();

    /// <summary>
    /// 医学管理
    /// </summary>
    public CountDetail Tensu13 { get; set; } = new CountDetail();

    /// <summary>
    /// 在宅
    /// </summary>
    public CountDetail Tensu14x { get; set; } = new CountDetail();

    /// <summary>
    /// 在宅薬剤
    /// </summary>
    public CountDetail Tensu1450 { get; set; } = new CountDetail();

    /// <summary>
    /// 投薬計
    /// </summary>
    public CountDetail Tensu2x { get; set; } = new CountDetail();

    /// <summary>
    /// 薬剤
    /// </summary>
    public CountDetail Tensu2100 { get; set; } = new CountDetail();

    /// <summary>
    /// 調剤
    /// </summary>
    public CountDetail Tensu2110 { get; set; } = new CountDetail();

    /// <summary>
    /// 処方
    /// </summary>
    public CountDetail Tensu2500 { get; set; } = new CountDetail();

    /// <summary>
    /// 麻毒
    /// </summary>
    public CountDetail Tensu2600 { get; set; } = new CountDetail();

    /// <summary>
    /// 調基
    /// </summary>
    public CountDetail Tensu2700 { get; set; } = new CountDetail();

    /// <summary>
    /// 注射計
    /// </summary>
    public CountDetail Tensu3x { get; set; } = new CountDetail();

    /// <summary>
    /// 皮下筋肉内注射
    /// </summary>
    public CountDetail Tensu31 { get; set; } = new CountDetail();

    /// <summary>
    /// 静脈内注射
    /// </summary>
    public CountDetail Tensu32 { get; set; } = new CountDetail();

    /// <summary>
    /// その他注射
    /// </summary>
    public CountDetail Tensu33 { get; set; } = new CountDetail();

    /// <summary>
    /// 注射(薬剤器材)
    /// </summary>
    public CountDetail Tensu39 { get; set; } = new CountDetail();

    /// <summary>
    /// 処置計
    /// </summary>
    public CountDetail Tensu4x { get; set; } = new CountDetail();

    /// <summary>
    /// 処置手技
    /// </summary>
    public CountDetail Tensu4000 { get; set; } = new CountDetail();

    /// <summary>
    /// 処置薬剤
    /// </summary>
    public CountDetail Tensu4010 { get; set; } = new CountDetail();

    /// <summary>
    /// 手術計
    /// </summary>
    public CountDetail Tensu5x { get; set; } = new CountDetail();

    /// <summary>
    /// 手術手技
    /// </summary>
    public CountDetail Tensu5000 { get; set; } = new CountDetail();

    /// <summary>
    /// 手術薬剤
    /// </summary>
    public CountDetail Tensu5010 { get; set; } = new CountDetail();

    /// <summary>
    /// 検査計
    /// </summary>
    public CountDetail Tensu6x { get; set; } = new CountDetail();

    /// <summary>
    /// 検査手技
    /// </summary>
    public CountDetail Tensu6000 { get; set; } = new CountDetail();

    /// <summary>
    /// 検査薬剤
    /// </summary>
    public CountDetail Tensu6010 { get; set; } = new CountDetail();

    /// <summary>
    /// 画像計
    /// </summary>
    public CountDetail Tensu7x { get; set; } = new CountDetail();

    /// <summary>
    /// 画像手技
    /// </summary>
    public CountDetail Tensu7000 { get; set; } = new CountDetail();

    /// <summary>
    /// 画像薬剤
    /// </summary>
    public CountDetail Tensu7010 { get; set; } = new CountDetail();

    /// <summary>
    /// 画像フィルム
    /// </summary>
    public CountDetail Tensu7f { get; set; } = new CountDetail();

    /// <summary>
    /// その他計
    /// </summary>
    public CountDetail Tensu8x { get; set; } = new CountDetail();

    /// <summary>
    /// その他(処方せん)
    /// </summary>
    public CountDetail Tensu8000 { get; set; } = new CountDetail();

    /// <summary>
    /// その他(その他)
    /// </summary>
    public CountDetail Tensu8010 { get; set; } = new CountDetail();

    /// <summary>
    /// その他(薬剤)
    /// </summary>
    public CountDetail Tensu8020 { get; set; } = new CountDetail();

    /// <summary>
    /// 合計①
    /// </summary>
    public CountDetail Total1 { get; set; } = new CountDetail();

    /// <summary>
    /// 構成比％
    /// </summary>
    public CountDetail Rate1 { get; set; } = new CountDetail();

    /// <summary>
    /// １日当り平均　①/③
    /// </summary>
    public CountDetail DayAvg1 { get; set; } = new CountDetail();

    /// <summary>
    /// １来院当り平均　①/④
    /// </summary>
    public CountDetail RaiinAvg1 { get; set; } = new CountDetail();

    /// <summary>
    /// １患者当り平均　①/⑤
    /// </summary>
    public CountDetail PtAvg1 { get; set; } = new CountDetail();

    /// <summary>
    /// 自費計
    /// </summary>
    public CountDetail Jihi { get; set; } = new CountDetail();

    /// <summary>
    /// 自費明細
    /// </summary>
    public List<CountDetail> JihiMeisais { get; set; } = new List<CountDetail>();

    /// <summary>
    /// 合計②
    /// </summary>
    public CountDetail Total2 { get; set; } = new CountDetail();

    /// <summary>
    /// 構成比％
    /// </summary>
    public CountDetail Rate2 { get; set; } = new CountDetail();

    /// <summary>
    /// １日当り平均　②/③
    /// </summary>
    public CountDetail DayAvg2 { get; set; } = new CountDetail();

    /// <summary>
    /// １来院当り平均　②/④
    /// </summary>
    public CountDetail RaiinAvg2 { get; set; } = new CountDetail();

    /// <summary>
    /// １患者当り平均　②/⑤
    /// </summary>
    public CountDetail PtAvg2 { get; set; } = new CountDetail();

    /// <summary>
    /// 稼働実日数　③
    /// </summary>
    public CountDetail DayCount { get; set; } = new CountDetail();

    /// <summary>
    /// 来院数　④
    /// </summary>
    public CountDetail RaiinCount { get; set; } = new CountDetail();

    /// <summary>
    /// 実人数　⑤
    /// </summary>
    public CountDetail PtCount { get; set; } = new CountDetail();

    /// <summary>
    /// １日平均来院　④/③
    /// </summary>
    public CountDetail DayRaiinAvg { get; set; } = new CountDetail();

    /// <summary>
    /// １人平均来院　④/⑤
    /// </summary>
    public CountDetail DayPtAvg { get; set; } = new CountDetail();

    /// <summary>
    /// 診療科略称
    /// </summary>
    public string KaSname { get; set; } = string.Empty;

    /// <summary>
    /// 担当医略称
    /// </summary>
    public string TantoSname { get; set; } = string.Empty;
}
