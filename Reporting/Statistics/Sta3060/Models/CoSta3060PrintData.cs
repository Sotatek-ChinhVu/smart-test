using Helper.Common;
using Reporting.Statistics.Enums;

namespace Reporting.Statistics.Sta3060.Models;

public class CoSta3060PrintData
{
    public CoSta3060PrintData(RowType rowType = RowType.Data)
    {
        RowType = rowType;
    }

    /// <summary>
    /// 行タイプ
    /// </summary>
    public RowType RowType { get; set; }

    /// <summary>
    /// 合計行のキャプション
    /// </summary>
    public string TotalCaption { get; set; }

    /// <summary>
    /// 集計区分
    /// </summary>
    public string ReportKbn { get; set; }

    /// <summary>
    /// 診察年月
    /// </summary>
    public int SinYm { get; set; }

    /// <summary>
    /// 診察年月 (yyyy/MM)
    /// </summary>
    public string SinYmFmt
    {
        get => CIUtil.SMonthToShowSMonth(SinYm);
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
    /// 初診件数
    /// </summary>
    public string SyosinCount { get; set; }

    /// <summary>
    /// 再診件数
    /// </summary>
    public string SaisinCount { get; set; }

    /// <summary>
    /// 初診率
    /// </summary>
    public string SyosinRate { get; set; }

    /// <summary>
    /// 合計件数
    /// </summary>
    public string TotalCount { get; set; }

    /// <summary>
    /// 実人数
    /// </summary>
    public string PtCount { get; set; }

    /// <summary>
    /// 合計点数
    /// </summary>
    public string TotalTensu { get; set; }

    /// <summary>
    /// 平均点数
    /// </summary>
    public string AvgTensu { get; set; }

    /// <summary>
    /// 行為点数 - 診察
    /// </summary>
    public string KouiTensu0 { get; set; }

    /// <summary>
    /// 行為点数 - 投薬
    /// </summary>
    public string KouiTensu1 { get; set; }

    /// <summary>
    /// 行為点数 - 注射
    /// </summary>
    public string KouiTensu2 { get; set; }

    /// <summary>
    /// 行為点数 - 処置
    /// </summary>
    public string KouiTensu3 { get; set; }

    /// <summary>
    /// 行為点数 - 手術
    /// </summary>
    public string KouiTensu4 { get; set; }

    /// <summary>
    /// 行為点数 - 検査
    /// </summary>
    public string KouiTensu5 { get; set; }

    /// <summary>
    /// 行為点数 - 画像
    /// </summary>
    public string KouiTensu6 { get; set; }

    /// <summary>
    /// 行為点数 - その他
    /// </summary>
    public string KouiTensu7 { get; set; }

    /// <summary>
    /// 行為点数 - 自費(円)
    /// </summary>
    public string KouiTensu8 { get; set; }

    /// <summary>
    /// 行為件数 - 診察
    /// </summary>
    public string KouiCount0 { get; set; }

    /// <summary>
    /// 行為件数 - 投薬
    /// </summary>
    public string KouiCount1 { get; set; }

    /// <summary>
    /// 行為件数 - 注射
    /// </summary>
    public string KouiCount2 { get; set; }

    /// <summary>
    /// 行為件数 - 処置
    /// </summary>
    public string KouiCount3 { get; set; }

    /// <summary>
    /// 行為件数 - 手術
    /// </summary>
    public string KouiCount4 { get; set; }

    /// <summary>
    /// 行為件数 - 検査
    /// </summary>
    public string KouiCount5 { get; set; }

    /// <summary>
    /// 行為件数 - 画像
    /// </summary>
    public string KouiCount6 { get; set; }

    /// <summary>
    /// 行為件数 - その他
    /// </summary>
    public string KouiCount7 { get; set; }

    /// <summary>
    /// 行為件数 - 自費(円)
    /// </summary>
    public string KouiCount8 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 初再診
    /// </summary>
    public string KouiTensuDetail0 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 医管
    /// </summary>
    public string KouiTensuDetail1 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 在宅
    /// </summary>
    public string KouiTensuDetail2 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 検査
    /// </summary>
    public string KouiTensuDetail3 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 画像
    /// </summary>
    public string KouiTensuDetail4 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 投薬
    /// </summary>
    public string KouiTensuDetail5 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 注射
    /// </summary>
    public string KouiTensuDetail6 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - リハ
    /// </summary>
    public string KouiTensuDetail7 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 精神
    /// </summary>
    public string KouiTensuDetail8 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 処置
    /// </summary>
    public string KouiTensuDetail9 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 手術
    /// </summary>
    public string KouiTensuDetail10 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 麻酔
    /// </summary>
    public string KouiTensuDetail11 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 放射
    /// </summary>
    public string KouiTensuDetail12 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 病理
    /// </summary>
    public string KouiTensuDetail13 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - その他
    /// </summary>
    public string KouiTensuDetail14 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 自費(円)
    /// </summary>
    public string KouiTensuDetail15 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 初再診
    /// </summary>
    public string KouiCountDetail0 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 医管
    /// </summary>
    public string KouiCountDetail1 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 在宅
    /// </summary>
    public string KouiCountDetail2 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 検査
    /// </summary>
    public string KouiCountDetail3 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 画像
    /// </summary>
    public string KouiCountDetail4 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 投薬
    /// </summary>
    public string KouiCountDetail5 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 注射
    /// </summary>
    public string KouiCountDetail6 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - リハ
    /// </summary>
    public string KouiCountDetail7 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 精神
    /// </summary>
    public string KouiCountDetail8 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 処置
    /// </summary>
    public string KouiCountDetail9 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 手術
    /// </summary>
    public string KouiCountDetail10 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 麻酔
    /// </summary>
    public string KouiCountDetail11 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 放射
    /// </summary>
    public string KouiCountDetail12 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 病理
    /// </summary>
    public string KouiCountDetail13 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - その他
    /// </summary>
    public string KouiCountDetail14 { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 自費(円)
    /// </summary>
    public string KouiCountDetail15 { get; set; }

    /// <summary>
    /// 患者負担合計
    /// </summary>
    public string TotalPtFutan { get; set; }

    /// <summary>
    /// 患者番号
    /// </summary>
    public string PtNum { get; set; }

    /// <summary>
    /// 患者氏名
    /// </summary>
    public string PtName { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 初再診 の内、初診のみ
    /// </summary>
    public string KouiTensuSyosin { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 初再診 の内、初診のみ
    /// </summary>
    public string KouiCountSyosin { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 初再診 の内、再診のみ
    /// </summary>
    public string KouiTensuSaisin { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 初再診 の内、再診のみ
    /// </summary>
    public string KouiCountSaisin { get; set; }

    /// <summary>
    /// 行為点数(詳細) - 初再診 の内、初診・再診以外
    /// </summary>
    public string KouiTensuSyosaiSonota { get; set; }

    /// <summary>
    /// 行為件数(詳細) - 初再診 の内、初診・再診以外
    /// </summary>
    public string KouiCountSyosaiSonota { get; set; }
}
