using Reporting.Statistics.Enums;

namespace Reporting.Statistics.Sta3062.Models;

public class CoSta3062PrintData
{
    public CoSta3062PrintData(RowType rowType = RowType.Data)
    {
        RowType = rowType;
    }

    /// <summary>
    /// 行タイプ
    /// </summary>
    public RowType RowType { get; set; }

    /// <summary>
    /// 集計区分(診療日)
    /// </summary>
    public string ReportKbn { get; set; } = string.Empty;

    /// <summary>
    /// 診察年月
    /// </summary>
    public int SinYm { get; set; }

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
    /// 合計点数
    /// </summary>
    public string TotalTensu { get; set; } = string.Empty;

    /// <summary>
    /// 来院数(合計件数)
    /// </summary>
    public string TotalCount { get; set; } = string.Empty;

    /// <summary>
    /// 実人数
    /// </summary>
    public string PtCount { get; set; } = string.Empty;

    /// <summary>
    /// 来院単価(点)(平均点数)
    /// </summary>
    public string AvgTensu { get; set; } = string.Empty;

    /// <summary>
    /// 来院単価(円)
    /// </summary>
    public string AvgTanka { get; set; } = string.Empty;

    /// <summary>
    /// 合計単価
    /// </summary>
    public string TotalTanka { get; set; } = string.Empty;

    /// <summary>
    /// 行為点数 - 初診
    /// </summary>
    public string KouiTensu0 { get; set; } = string.Empty;

    /// <summary>
    /// 行為点数 - 再診
    /// </summary>
    public string KouiTensu1 { get; set; } = string.Empty;

    /// <summary>
    /// 行為点数 - 医学管理
    /// </summary>
    public string KouiTensu2 { get; set; } = string.Empty;

    /// <summary>
    /// 行為点数 - 在宅
    /// </summary>
    public string KouiTensu3 { get; set; } = string.Empty;

    /// <summary>
    /// 行為点数 - 薬剤器材
    /// </summary>
    public string KouiTensu4 { get; set; } = string.Empty;

    /// <summary>
    /// 行為点数 - 投薬
    /// </summary>
    public string KouiTensu5 { get; set; } = string.Empty;

    /// <summary>
    /// 行為点数 - 注射
    /// </summary>
    public string KouiTensu6 { get; set; } = string.Empty;

    /// <summary>
    /// 行為点数 - 処置
    /// </summary>
    public string KouiTensu7 { get; set; } = string.Empty;

    /// <summary>
    /// 行為点数 - 手術
    /// </summary>
    public string KouiTensu8 { get; set; } = string.Empty;

    /// <summary>
    /// 行為点数 - 検査
    /// </summary>
    public string KouiTensu9 { get; set; } = string.Empty;

    /// <summary>
    /// 行為点数 - 画像
    /// </summary>
    public string KouiTensu10 { get; set; } = string.Empty;

    /// <summary>
    /// 行為点数 - その他
    /// </summary>
    public string KouiTensu11 { get; set; } = string.Empty;

    /// <summary>
    /// 行為点数 - 自費(円)
    /// </summary>
    public string KouiTensu12 { get; set; } = string.Empty;

    /// <summary>
    /// 行為件数 - 初診
    /// </summary>
    public string KouiCount0 { get; set; } = string.Empty;

    /// <summary>
    /// 行為件数 - 再診
    /// </summary>
    public string KouiCount1 { get; set; } = string.Empty;

    /// <summary>
    /// 行為件数 - 医学管理
    /// </summary>
    public string KouiCount2 { get; set; } = string.Empty;

    /// <summary>
    /// 行為件数 - 在宅
    /// </summary>
    public string KouiCount3 { get; set; } = string.Empty;

    /// <summary>
    /// 行為件数 - 薬剤器材
    /// </summary>
    public string KouiCount4 { get; set; } = string.Empty;

    /// <summary>
    /// 行為件数 - 投薬
    /// </summary>
    public string KouiCount5 { get; set; } = string.Empty;

    /// <summary>
    /// 行為件数 - 注射
    /// </summary>
    public string KouiCount6 { get; set; } = string.Empty;

    /// <summary>
    /// 行為件数 - 処置
    /// </summary>
    public string KouiCount7 { get; set; } = string.Empty;

    /// <summary>
    /// 行為件数 - 手術
    /// </summary>
    public string KouiCount8 { get; set; } = string.Empty;

    /// <summary>
    /// 行為件数 - 検査
    /// </summary>
    public string KouiCount9 { get; set; } = string.Empty;

    /// <summary>
    /// 行為件数 - 画像
    /// </summary>
    public string KouiCount10 { get; set; } = string.Empty;

    /// <summary>
    /// 行為件数 - その他
    /// </summary>
    public string KouiCount11 { get; set; } = string.Empty;

    /// <summary>
    /// 行為件数 - 自費(円)
    /// </summary>
    public string KouiCount12 { get; set; } = string.Empty;

    /// <summary>
    /// 行為単価 - 初診
    /// </summary>
    public string KouiTanka0 { get; set; } = string.Empty;

    /// <summary>
    /// 行為単価 - 再診
    /// </summary>
    public string KouiTanka1 { get; set; } = string.Empty;

    /// <summary>
    /// 行為単価 - 医学管理
    /// </summary>
    public string KouiTanka2 { get; set; } = string.Empty;

    /// <summary>
    /// 行為単価 - 在宅
    /// </summary>
    public string KouiTanka3 { get; set; } = string.Empty;

    /// <summary>
    /// 行為単価 - 薬剤器材
    /// </summary>
    public string KouiTanka4 { get; set; } = string.Empty;

    /// <summary>
    /// 行為単価 - 投薬
    /// </summary>
    public string KouiTanka5 { get; set; } = string.Empty;

    /// <summary>
    /// 行為単価 - 注射
    /// </summary>
    public string KouiTanka6 { get; set; } = string.Empty;

    /// <summary>
    /// 行為単価 - 処置
    /// </summary>
    public string KouiTanka7 { get; set; } = string.Empty;

    /// <summary>
    /// 行為単価 - 手術
    /// </summary>
    public string KouiTanka8 { get; set; } = string.Empty;

    /// <summary>
    /// 行為単価 - 検査
    /// </summary>
    public string KouiTanka9 { get; set; } = string.Empty;

    /// <summary>
    /// 行為単価 - 画像
    /// </summary>
    public string KouiTanka10 { get; set; } = string.Empty;

    /// <summary>
    /// 行為単価 - その他
    /// </summary>
    public string KouiTanka11 { get; set; } = string.Empty;
}
