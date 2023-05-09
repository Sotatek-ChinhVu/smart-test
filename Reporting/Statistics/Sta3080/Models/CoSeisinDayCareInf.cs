namespace Reporting.Statistics.Sta3080.Models;

public class CoSeisinDayCareInf
{

    /// <summary>
    /// 診療年月
    /// </summary>
    public int SinYm { get; set; }

    /// <summary>
    /// 患者番号
    /// </summary>
    public long PtNum { get; set; }

    /// <summary>
    /// カナ氏名
    /// </summary>
    public string KanaName { get; set; } = string.Empty;

    /// <summary>
    /// 氏名
    /// </summary>
    public string Name { get; set; } = string.Empty;

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
    /// 初回算定月
    /// </summary>
    public string SyokaiYm { get; set; } = string.Empty;

}
