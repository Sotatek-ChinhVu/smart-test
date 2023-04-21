using Entity.Tenant;

namespace Reporting.Accounting.Model;

public class CoPtGrpNameMstModel
{
    public PtGrpNameMst PtGrpNameMst { get; } = new();

    public CoPtGrpNameMstModel(PtGrpNameMst ptGrpNameMst)
    {
        PtGrpNameMst = ptGrpNameMst;
    }

    /// <summary>
    /// 患者分類名称マスタ
    /// </summary>
    /// <summary>
    /// 医療機関識別ID
    /// </summary>
    public int HpId
    {
        get { return PtGrpNameMst.HpId; }
    }

    /// <summary>
    /// 分類番号
    /// </summary>
    public int GrpId
    {
        get { return PtGrpNameMst.GrpId; }
    }

    /// <summary>
    /// 並び順
    /// </summary>
    public int SortNo
    {
        get { return PtGrpNameMst.SortNo; }
    }

    /// <summary>
    /// 分類名
    /// </summary>
    public string GrpName
    {
        get { return PtGrpNameMst.GrpName ?? string.Empty; }
    }

    /// <summary>
    /// 削除区分
    ///  1:削除 
    /// </summary>
    public int IsDeleted
    {
        get { return PtGrpNameMst.IsDeleted; }
    }

}

