using Reporting.Statistics.Enums;

namespace Reporting.Statistics.Sta3080.Models;

public class CoSta3080PrintData
{
    public CoSta3080PrintData(RowType rowType = RowType.Data)
    {
        RowType = rowType;
    }

    /// <summary>
    /// 行タイプ
    /// </summary>
    public RowType RowType { get; set; }

    /// <summary>
    /// 合計行のタイトル
    /// </summary>
    public string TotalKbn { get; set; } = string.Empty;

    /// <summary>
    /// 合計行のキャプション
    /// </summary>
    public string TotalCaption { get; set; } = string.Empty;

    /// <summary>
    /// 合計行の件数
    /// </summary>
    public string TotalVal { get; set; } = string.Empty;

    /// <summary>
    /// 診療年月
    /// </summary>
    public string SinYm { get; set; } = string.Empty;

    /// <summary>
    /// 患者番号
    /// </summary>
    public string PtNum { get; set; } = string.Empty;

    /// <summary>
    /// 患者番号Key
    /// </summary>
    public long PtNumKey { get; set; }

    /// <summary>
    /// カナ氏名
    /// </summary>
    public string PtKanaName { get; set; } = string.Empty;

    /// <summary>
    /// 氏名
    /// </summary>
    public string PtName { get; set; } = string.Empty;

    /// <summary>
    /// 診療行為コード
    /// </summary>
    public string ItemCd { get; set; } = string.Empty;

    /// <summary>
    /// 診療行為名称
    /// </summary>
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 回数
    /// </summary>
    public string OdrCount { get; set; } = string.Empty;

    /// <summary>
    /// 患者回数計
    /// </summary>
    public string TotalOdrCount { get; set; } = string.Empty;

    /// <summary>
    /// 初回算定月
    /// </summary>
    public string SyokaiYm { get; set; } = string.Empty;

    /// <summary>
    /// 明細区分
    /// 0:明細 1:小計 2:合計
    /// </summary>
    public int MeisaiKbn { get; set; }

    /// <summary>
    /// 計見出し
    /// </summary>
    public string MeisaiKbnName
    {
        get
        {
            switch (MeisaiKbn)
            {
                case 0: return "明細";
                case 1: return "小計";
                case 2: return "合計";
            }
            return "";
        }
    }

    /// <summary>
    /// 1回以上実施患者数
    /// </summary>
    public string PtCount1 { get; set; } = string.Empty;

    /// <summary>
    /// 14回以上実施患者数
    /// </summary>
    public string PtCount14 { get; set; } = string.Empty;

    /// <summary>
    /// 経過月数
    /// </summary>
    public string KeikaMon { get; set; } = string.Empty;

    /// <summary>
    /// 経過月数合計
    /// </summary>
    public string TotalKeikaMon { get; set; } = string.Empty;
}
