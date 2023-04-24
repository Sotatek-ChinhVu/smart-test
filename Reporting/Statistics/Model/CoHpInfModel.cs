using Entity.Tenant;
namespace Reporting.Statistics.Model;

public class CoHpInfModel
{
    public HpInf HpInf { get; private set; }

    public CoHpInfModel(HpInf hpInf)
    {
        HpInf = hpInf;
    }

    /// <summary>
    /// 医療機関名
    /// </summary>
    public string HpName
    {
        get => HpInf.HpName ?? string.Empty;
    }

    /// <summary>
    /// 都道府県番号
    /// </summary>
    public int PrefNo
    {
        get => HpInf.PrefNo;
    }
}
