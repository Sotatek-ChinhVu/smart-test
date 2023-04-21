using Entity.Tenant;

namespace Reporting.Accounting.Model;

public class CoPtMemoModel
{
    public PtMemo PtMemo { get; }

    public CoPtMemoModel()
    {
        PtMemo = new();
    }

    public CoPtMemoModel(PtMemo ptMemo)
    {
        PtMemo = ptMemo;
    }

    /// <summary>
    /// 患者メモ
    /// </summary>
    /// <summary>
    /// 医療機関識別ID
    /// </summary>
    public int HpId
    {
        get { return PtMemo == null ? 0 : PtMemo.HpId; }
    }

    /// <summary>
    /// 患者ID
    ///  患者を識別するためのシステム固有の番号      
    /// </summary>
    public long PtId
    {
        get { return PtMemo == null ? 0 : PtMemo.PtId; }
    }

    /// <summary>
    /// 連番
    /// </summary>
    public long SeqNo
    {
        get { return PtMemo == null ? 0 : PtMemo.SeqNo; }
    }

    /// <summary>
    /// メモ
    /// </summary>
    public string Memo
    {
        get { return PtMemo.Memo ?? string.Empty; }
    }

    /// <summary>
    /// 削除区分
    /// </summary>
    public int IsDeleted
    {
        get { return PtMemo == null ? 0 : PtMemo.IsDeleted; }
    }

}
