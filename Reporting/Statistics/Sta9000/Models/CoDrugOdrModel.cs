namespace Reporting.Statistics.Sta9000.Models;

public class CoDrugOdrModel
{
    /// <summary>
    /// 患者ID
    /// </summary>
    public long PtId { get; set; }

    /// <summary>
    /// 診療日
    /// </summary>
    public int SinDate { get; set; }

    /// <summary>
    /// 診療行為コード
    /// </summary>
    public string ItemCd { get; set; } = string.Empty;

    /// <summary>
    /// 漢字名称
    /// </summary>
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 単位名称
    /// </summary>
    public string UnitName { get; set; } = string.Empty;

    /// <summary>
    /// 数量
    /// </summary>
    public double Suryo { get; set; }
}
