using Entity.Tenant;

namespace Reporting.Accounting.Model;

public class CoPtGrpInfModel
{
    public PtGrpInf PtGrpInf { get; }
    public PtGrpItem PtGrpItem { get; }
    public CoPtGrpInfModel(PtGrpInf ptGrpInf, PtGrpItem ptGrpItem)
    {
        PtGrpInf = ptGrpInf;
        PtGrpItem = ptGrpItem;
    }

    /// <summary>
    /// 患者分類情報
    /// </summary>
    /// <summary>
    /// 医療機関識別ID
    /// </summary>
    public int HpId
    {
        get { return PtGrpInf.HpId; }
    }

    /// <summary>
    /// 患者ID
    ///  患者を識別するためのシステム固有の番号
    /// </summary>
    public long PtId
    {
        get { return PtGrpInf.PtId; }
    }

    /// <summary>
    /// 分類番号
    /// </summary>
    public int GroupId
    {
        get { return PtGrpInf.GroupId; }
    }

    /// <summary>
    /// 連番
    /// </summary>
    public long SeqNo
    {
        get { return PtGrpInf.SeqNo; }
    }

    /// <summary>
    /// 並び順
    /// </summary>
    public int SortNo
    {
        get { return PtGrpInf.SortNo; }
    }

    /// <summary>
    /// 分類項目コード
    /// </summary>
    public string GroupCode
    {
        get { return PtGrpInf.GroupCode ?? string.Empty; }
    }

    /// <summary>
    /// 削除区分
    /// </summary>
    public int IsDeleted
    {
        get { return PtGrpInf.IsDeleted; }
    }

    /// <summary>
    /// グループコード名称
    /// </summary>
    public string GrpCdName
    {
        get { return PtGrpItem.GrpCodeName ?? string.Empty; }
    }
}