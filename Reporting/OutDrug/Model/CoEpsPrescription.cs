using Entity.Tenant;

namespace Reporting.OutDrug.Model;

public class CoEpsPrescription
{
    public EpsPrescription EpsPrescription { get; set; }
    public CoEpsPrescription(EpsPrescription epsPrescription)
    {
        EpsPrescription = epsPrescription;
    }

    /// <summary>
    /// 引換番号
    /// </summary>
    public string AccessCode
    {
        get => EpsPrescription.AccessCode ?? string.Empty;
    }

    /// <summary>
    /// 発行形態
    /// </summary>
    public int IssueType
    {
        get => EpsPrescription.IssueType;
    }

    /// <summary>
    /// リフィル回数
    /// </summary>
    public int RefileCount
    {
        get => EpsPrescription.RefileCount;
    }

    /// <summary>
    /// 保険者番号
    /// </summary>
    public string HokensyaNo
    {
        get => EpsPrescription.HokensyaNo ?? string.Empty;
    }

    /// <summary>
    /// 被保険者記号
    /// </summary>
    public string Kigo
    {
        get => EpsPrescription.Kigo ?? string.Empty;
    }

    /// <summary>
    /// 被保険者番号
    /// </summary>
    public string Bango
    {
        get => EpsPrescription.Bango ?? string.Empty;
    }

    /// <summary>
    /// 被保険者枝番
    /// </summary>
    public string EdaNo
    {
        get => EpsPrescription.EdaNo ?? string.Empty;
    }

    /// <summary>
    /// 公費受給者番号
    /// </summary>
    public string KohiJyukyusyaNo
    {
        get => EpsPrescription.KohiJyukyusyaNo ?? string.Empty;
    }

    /// <summary>
    /// 公費負担者番号
    /// </summary>
    public string KohiFutansyaNo
    {
        get => EpsPrescription.KohiFutansyaNo ?? string.Empty;
    }

}
