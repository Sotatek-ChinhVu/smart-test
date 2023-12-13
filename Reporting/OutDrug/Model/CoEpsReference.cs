using Entity.Tenant;

namespace Reporting.OutDrug.Model;

public class CoEpsReference
{
    public EpsReference EpsReference { get; set; }

    public CoEpsReference(EpsReference epsReference)
    {
        EpsReference = epsReference;
    }

    /// <summary>
    /// 患者ID
    /// </summary>
    public long PtId
    {
        get => EpsReference.PtId;
    }

    /// <summary>
    /// 来院番号
    /// </summary>
    public long RaiinNo
    {
        get => EpsReference.RaiinNo;
    }

    /// <summary>
    /// 診療日
    /// </summary>
    public int SinDate
    {
        get => EpsReference.SinDate;
    }

    /// <summary>
    /// 処方箋ID
    /// </summary>
    public string PrescriptionId
    {
        get => EpsReference.PrescriptionId ?? string.Empty;
    }

    /// <summary>
    /// 処方内容控え
    /// </summary>
    public string PrescriptionReferenceInformation
    {
        get => EpsReference.PrescriptionReferenceInformation ?? string.Empty;
    }

}
