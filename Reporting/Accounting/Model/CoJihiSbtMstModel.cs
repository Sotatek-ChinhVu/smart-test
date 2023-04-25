using Entity.Tenant;

namespace Reporting.Accounting.Model;

public class CoJihiSbtMstModel
{
    public JihiSbtMst JihiSbtMst { get; }

    public CoJihiSbtMstModel(JihiSbtMst jihiSbtMst)
    {
        JihiSbtMst = jihiSbtMst;
    }

    /// <summary>
    /// 医療機関識別ID
    /// 
    /// </summary>
    public int HpId
    {
        get { return JihiSbtMst.HpId; }
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
    /// 
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

    /// <summary>
    /// 削除区分
    /// 1:削除
    /// </summary>
    public int IsDeleted
    {
        get { return JihiSbtMst.IsDeleted; }
    }


}

