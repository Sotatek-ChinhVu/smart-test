using Entity.Tenant;

namespace Reporting.Accounting.Model;

public class PtGrpNameMstModel
{
    public PtGrpNameMst PtGrpNameMst { get; }

    public PtGrpNameMstModel(PtGrpNameMst ptGrpNameMst, IEnumerable<PtGrpItemModel> enumerable)
    {
        PtGrpNameMst = ptGrpNameMst;
        List<PtGrpItemModel> models = new List<PtGrpItemModel>()
        {
            new PtGrpItemModel(new PtGrpItem()),
            new PtGrpItemModel(new PtGrpItem()
            {
                GrpCodeName = "すべて",
                GrpCode = "al"
            })
        };
        models.AddRange(enumerable);
        PtGrpItemModels = new List<PtGrpItemModel>(models);
    }

    public List<PtGrpItemModel> PtGrpItemModels { get; set; }

    public string GrpItemSelected { get; set; } = string.Empty;

    public bool IsSelecteAllGrpItem
    {
        get => GrpItemSelected == "al";
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
        set
        {
            if (PtGrpNameMst.HpId == value) return;
            PtGrpNameMst.HpId = value;
        }
    }

    /// <summary>
    /// 分類番号
    /// </summary>
    public int GrpId
    {
        get { return PtGrpNameMst.GrpId; }
        set
        {
            if (PtGrpNameMst.GrpId == value) return;
            PtGrpNameMst.GrpId = value;
        }
    }

    /// <summary>
    /// 並び順
    /// </summary>
    public int SortNo
    {
        get { return PtGrpNameMst.SortNo; }
        set
        {
            if (PtGrpNameMst.SortNo == value) return;
            PtGrpNameMst.SortNo = value;
        }
    }

    /// <summary>
    /// 分類名
    /// </summary>
    public string GrpName
    {
        get { return PtGrpNameMst.GrpName ?? string.Empty; }
        set
        {
            if (PtGrpNameMst.GrpName == value) return;
            PtGrpNameMst.GrpName = value;
        }
    }

    /// <summary>
    /// 削除区分
    ///  1:削除 
    /// </summary>
    public int IsDeleted
    {
        get { return PtGrpNameMst.IsDeleted; }
        set
        {
            if (PtGrpNameMst.IsDeleted == value) return;
            PtGrpNameMst.IsDeleted = value;
        }
    }

    /// <summary>
    /// 作成日時 
    /// </summary>
    public DateTime CreateDate
    {
        get { return PtGrpNameMst.CreateDate; }
        set
        {
            if (PtGrpNameMst.CreateDate == value) return;
            PtGrpNameMst.CreateDate = value;
        }
    }

    /// <summary>
    /// 作成者  
    /// </summary>
    public int CreateId
    {
        get { return PtGrpNameMst.CreateId; }
        set
        {
            if (PtGrpNameMst.CreateId == value) return;
            PtGrpNameMst.CreateId = value;
        }
    }

    /// <summary>
    /// 作成端末   
    /// </summary>
    public string CreateMachine
    {
        get { return PtGrpNameMst.CreateMachine ?? string.Empty; }
        set
        {
            if (PtGrpNameMst.CreateMachine == value) return;
            PtGrpNameMst.CreateMachine = value;
        }
    }

    /// <summary>
    /// 更新日時   
    /// </summary>
    public DateTime UpdateDate
    {
        get { return PtGrpNameMst.UpdateDate; }
        set
        {
            if (PtGrpNameMst.UpdateDate == value) return;
            PtGrpNameMst.UpdateDate = value;
        }
    }

    /// <summary>
    /// 更新者   
    /// </summary>
    public int UpdateId
    {
        get { return PtGrpNameMst.UpdateId; }
        set
        {
            if (PtGrpNameMst.UpdateId == value) return;
            PtGrpNameMst.UpdateId = value;
        }
    }

    /// <summary>
    /// 更新端末   
    /// </summary>
    public string UpdateMachine
    {
        get { return PtGrpNameMst.UpdateMachine ?? string.Empty; }
        set
        {
            if (PtGrpNameMst.UpdateMachine == value) return;
            PtGrpNameMst.UpdateMachine = value;
        }
    }
}
