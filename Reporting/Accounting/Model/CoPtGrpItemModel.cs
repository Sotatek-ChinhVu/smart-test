using Entity.Tenant;

namespace Reporting.Accounting.Model;

public class CoPtGrpItemModel
{
    public PtGrpItem PtGrpItem { get; }

    public CoPtGrpItemModel(PtGrpItem ptGrpItem)
    {
        PtGrpItem = ptGrpItem;
    }

    /// <summary>
    /// 患者分類項目
    /// </summary>
    /// <summary>
    /// 医療機関識別ID
    /// </summary>
    public int HpId
    {
        get { return PtGrpItem.HpId; }
    }

    /// <summary>
    /// 分類番号
    /// </summary>
    public int GrpId
    {
        get { return PtGrpItem.GrpId; }
    }

    /// <summary>
    /// 分類項目コード
    /// </summary>
    public string GrpCode
    {
        get { return PtGrpItem.GrpCode; }
    }

    /// <summary>
    /// 連番
    /// </summary>
    public long SeqNo
    {
        get { return PtGrpItem.SeqNo; }
    }

    /// <summary>
    /// 分類項目名称
    /// </summary>
    public string GrpCodeName
    {
        get { return PtGrpItem.GrpCodeName ?? string.Empty; }
    }

    /// <summary>
    /// 並び順
    /// </summary>
    public int SortNo
    {
        get { return PtGrpItem.SortNo; }
    }

    /// <summary>
    /// 削除区分
    ///  1:削除  
    /// </summary>
    public int IsDeleted
    {
        get { return PtGrpItem.IsDeleted; }
    }

    /// <summary>
    /// 作成日時 
    /// </summary>
    public DateTime CreateDate
    {
        get { return PtGrpItem.CreateDate; }
    }

    /// <summary>
    /// 作成者  
    /// </summary>
    public int CreateId
    {
        get { return PtGrpItem.CreateId; }
    }

    /// <summary>
    /// 作成端末   
    /// </summary>
    public string CreateMachine
    {
        get { return PtGrpItem.CreateMachine ?? string.Empty; }
    }

    /// <summary>
    /// 更新日時   
    /// </summary>
    public DateTime UpdateDate
    {
        get { return PtGrpItem.UpdateDate; }
    }

    /// <summary>
    /// 更新者   
    /// </summary>
    public int UpdateId
    {
        get { return PtGrpItem.UpdateId; }
    }

    /// <summary>
    /// 更新端末   
    /// </summary>
    public string UpdateMachine
    {
        get { return PtGrpItem.UpdateMachine ?? string.Empty; }
    }


}

