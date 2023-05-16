using Entity.Tenant;

namespace Reporting.Statistics.Sta3061.Models;

public class CoJihiSbtMstModel
{
    public JihiSbtMst JihiSbtMst { get; }

    public CoJihiSbtMstModel(JihiSbtMst jihiSbtMst)
    {
        JihiSbtMst = jihiSbtMst;
    }

    /// <summary>
    /// 自費種別
    /// </summary>
    public int JihiSbt
    {
        get { return JihiSbtMst.JihiSbt; }
    }

    /// <summary>
    /// 種別名
    /// </summary>
    public string Name
    {
        get { return JihiSbtMst.Name ?? string.Empty; }
    }
}
