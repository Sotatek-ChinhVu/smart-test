using Entity.Tenant;

namespace Reporting.Receipt.Models;

public class PtKyuseiModel
{
    public PtKyusei PtKyusei { get; } = null;

    public PtKyuseiModel(PtKyusei ptKyusei)
    {
        PtKyusei = ptKyusei;
    }

    /// <summary>
    /// 旧姓情報
    /// </summary>
    /// <summary>
    /// 医療機関識別ID
    /// </summary>
    public int HpId
    {
        get { return PtKyusei.HpId; }
    }

    /// <summary>
    /// 患者ID
    ///  患者を識別するためのシステム固有の番号
    /// </summary>
    public long PtId
    {
        get { return PtKyusei.PtId; }
    }

    /// <summary>
    /// 連番
    /// </summary>
    public long SeqNo
    {
        get { return PtKyusei.SeqNo; }
    }

    /// <summary>
    /// カナ氏名
    /// </summary>
    public string KanaName
    {
        get { return PtKyusei.KanaName ?? string.Empty; }
    }

    /// <summary>
    /// 氏名
    /// </summary>
    public string Name
    {
        get { return PtKyusei.Name ?? string.Empty; }
    }

    /// <summary>
    /// 終了日
    ///  患者氏名が変更された日    
    /// </summary>
    public int EndDate
    {
        get { return PtKyusei.EndDate; }
    }

    /// <summary>
    /// 削除区分
    ///  1:削除
    /// </summary>
    public int IsDeleted
    {
        get { return PtKyusei.IsDeleted; }
    }

    /// <summary>
    /// 作成日時 
    /// </summary>
    public DateTime CreateDate
    {
        get { return PtKyusei.CreateDate; }
    }

    /// <summary>
    /// 作成者  
    /// </summary>
    public int CreateId
    {
        get { return PtKyusei.CreateId; }
    }

    /// <summary>
    /// 作成端末   
    /// </summary>
    public string CreateMachine
    {
        get { return PtKyusei.CreateMachine ?? string.Empty; }
    }

    /// <summary>
    /// 更新日時   
    /// </summary>
    public DateTime UpdateDate
    {
        get { return PtKyusei.UpdateDate; }
    }

    /// <summary>
    /// 更新者   
    /// </summary>
    public int UpdateId
    {
        get { return PtKyusei.UpdateId; }
    }

    /// <summary>
    /// 更新端末   
    /// </summary>
    public string UpdateMachine
    {
        get { return PtKyusei.UpdateMachine ?? string.Empty; }
    }


}
