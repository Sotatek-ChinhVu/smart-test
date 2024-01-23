using Entity.Tenant;
using Helper.Extension;

namespace Reporting.ReceTarget.Model;

public class CoReceInfModel
{
    public ReceInf ReceInf { get; }
    public PtInf PtInf { get; }

    public CoReceInfModel(ReceInf receInf, PtInf ptInf)
    {
        ReceInf = receInf;
        PtInf = ptInf;
    }

    /// <summary>
    /// 患者番号
    /// </summary>
    public long PtNum
    {
        get => PtInf.PtNum.AsLong();
    }
    /// <summary>
    /// 患者氏名
    /// </summary>
    public string PtName
    {
        get => PtInf.Name ?? string.Empty;
    }
    /// <summary>
    /// 保険区分
    /// </summary>
    public int HokenKbn
    {
        get
        {
            if (ReceInf.HokenKbn == 0)
            {
                return 999;
            }
            return ReceInf.HokenKbn;
        }
    }
    /// <summary>
    /// レセ種別
    /// </summary>
    public string ReceSbt
    {
        get => ReceInf.ReceSbt ?? string.Empty;
    }
}
