using Entity.Tenant;

namespace Reporting.MedicalRecordWebId.Model;

public class CoHpInfModel
{
    public HpInf HpInf { get; }

    public CoHpInfModel(HpInf hpInf)
    {
        HpInf = hpInf;
    }

    /// <summary>
    /// 医療機関名
    /// </summary>
    public string HpName => HpInf.HpName ?? string.Empty;
}
