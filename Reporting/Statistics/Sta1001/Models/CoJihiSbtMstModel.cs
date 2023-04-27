using Entity.Tenant;

namespace Reporting.Statistics.Sta1001.Models;

public class CoJihiSbtMstModel
{
    public JihiSbtMst JihiSbtMst { get; private set; }

    public CoJihiSbtMstModel(JihiSbtMst jihiSbtMst)
    {
        JihiSbtMst = jihiSbtMst;
    }

    /// <summary>
    /// 自費種別
    /// TEN_MST.自費種別
    /// </summary>
    public int JihiSbt
    {
        get { return JihiSbtMst.JihiSbt; }
    }

    /// <summary>
    /// 並び順
    /// </summary>
    public int SortNo
    {
        get { return JihiSbtMst.SortNo; }
    }

    /// <summary>
    /// 種別名
    /// 
    /// </summary>
    public string Name
    {
        get { return JihiSbtMst.Name ?? string.Empty; }
    }

}
