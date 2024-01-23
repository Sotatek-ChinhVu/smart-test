using Entity.Tenant;
using Helper.Extension;

namespace Reporting.MedicalRecordWebId.Model;

public class CoPtInfModel
{
    public PtInf PtInf { get; }

    public CoPtInfModel(PtInf ptInf)
    {
        PtInf = ptInf;
    }

    /// <summary>
    /// 患者ID
    ///     患者を識別するためのシステム固有の番号
    /// </summary>
    public long PtId => PtInf.PtId;

    /// <summary>
    /// 患者番号
    ///  医療機関が患者特定するための番号
    /// </summary>
    public long PtNum => PtInf.PtNum.AsLong();

    /// <summary>
    /// 氏名
    /// </summary>
    public string Name => PtInf.Name ?? string.Empty;

    /// <summary>
    /// カナ氏名
    /// </summary>
    public string KanaName => PtInf.KanaName ?? string.Empty;

    /// <summary>
    /// 生年月日
    ///     yyyymmdd
    /// </summary>
    public int Birthday => PtInf.Birthday;
}
