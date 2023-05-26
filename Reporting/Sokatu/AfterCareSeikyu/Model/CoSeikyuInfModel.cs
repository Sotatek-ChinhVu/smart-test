namespace Reporting.Sokatu.AfterCareSeikyu.Model;

public class CoSeikyuInfModel
{
    /// <summary>
    /// 請求金額
    /// </summary>
    public int SeikyuGaku { get; set; }

    /// <summary>
    /// 内訳書添付枚数
    /// </summary>
    public int MeisaiCount { get; set; }

    /// <summary>
    /// 代表者名
    /// </summary>
    public string DaihyoName { get; set; } = string.Empty;

    /// <summary>
    /// 請求人数（代表者以外の人数）
    /// </summary>
    public int SeikyuNinzu { get; set; }
}
