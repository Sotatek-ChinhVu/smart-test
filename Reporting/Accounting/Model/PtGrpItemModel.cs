using Entity.Tenant;

namespace Reporting.Accounting.Model;

public class PtGrpItemModel
{
    public PtGrpItem PtGrpItem { get; }

    public PtGrpItemModel(PtGrpItem ptGrpItem)
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
        set
        {
            if (PtGrpItem.HpId == value) return;
            PtGrpItem.HpId = value;
        }
    }

    /// <summary>
    /// 分類番号
    /// </summary>
    public int GrpId
    {
        get { return PtGrpItem.GrpId; }
        set
        {
            if (PtGrpItem.GrpId == value) return;
            PtGrpItem.GrpId = value;
        }
    }

    /// <summary>
    /// 分類項目コード
    /// </summary>
    public string GrpCode
    {
        get { return PtGrpItem.GrpCode; }
        set
        {
            if (PtGrpItem.GrpCode == value) return;
            PtGrpItem.GrpCode = value;
        }
    }

    /// <summary>
    /// 連番
    /// </summary>
    public long SeqNo
    {
        get { return PtGrpItem.SeqNo; }
        set
        {
            if (PtGrpItem.SeqNo == value) return;
            PtGrpItem.SeqNo = value;
        }
    }

    /// <summary>
    /// 分類項目名称
    /// </summary>
    public string GrpCodeName
    {
        get { return PtGrpItem.GrpCodeName ?? string.Empty; }
        set
        {
            if (PtGrpItem.GrpCodeName == value) return;
            PtGrpItem.GrpCodeName = value;
        }
    }
    /// <summary>
    /// 並び順
    /// </summary>
    public int SortNo
    {
        get { return PtGrpItem.SortNo; }
        set
        {
            if (PtGrpItem.SortNo == value) return;
            PtGrpItem.SortNo = value;
        }
    }

    /// <summary>
    /// 削除区分
    ///  1:削除  
    /// </summary>
    public int IsDeleted
    {
        get { return PtGrpItem.IsDeleted; }
        set
        {
            if (PtGrpItem.IsDeleted == value) return;
            PtGrpItem.IsDeleted = value;
        }
    }

    /// <summary>
    /// 作成日時 
    /// </summary>
    public DateTime CreateDate
    {
        get { return PtGrpItem.CreateDate; }
        set
        {
            if (PtGrpItem.CreateDate == value) return;
            PtGrpItem.CreateDate = value;
        }
    }

    /// <summary>
    /// 作成者  
    /// </summary>
    public int CreateId
    {
        get { return PtGrpItem.CreateId; }
        set
        {
            if (PtGrpItem.CreateId == value) return;
            PtGrpItem.CreateId = value;
        }
    }

    /// <summary>
    /// 作成端末   
    /// </summary>
    public string CreateMachine
    {
        get { return PtGrpItem.CreateMachine ?? string.Empty; }
        set
        {
            if (PtGrpItem.CreateMachine == value) return;
            PtGrpItem.CreateMachine = value;
        }
    }

    /// <summary>
    /// 更新日時   
    /// </summary>
    public DateTime UpdateDate
    {
        get { return PtGrpItem.UpdateDate; }
        set
        {
            if (PtGrpItem.UpdateDate == value) return;
            PtGrpItem.UpdateDate = value;
        }
    }

    /// <summary>
    /// 更新者   
    /// </summary>
    public int UpdateId
    {
        get { return PtGrpItem.UpdateId; }
        set
        {
            if (PtGrpItem.UpdateId == value) return;
            PtGrpItem.UpdateId = value;
        }
    }

    /// <summary>
    /// 更新端末   
    /// </summary>
    public string UpdateMachine
    {
        get { return PtGrpItem.UpdateMachine ?? string.Empty; }
        set
        {
            if (PtGrpItem.UpdateMachine == value) return;
            PtGrpItem.UpdateMachine = value;
        }
    }
}
