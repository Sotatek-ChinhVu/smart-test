using Entity.Tenant;

namespace Reporting.Accounting.Model;

public class CoKarteInfModel
{
    public KarteInf KarteInf { get; }

    public CoKarteInfModel(KarteInf karteInf)
    {
        KarteInf = karteInf;
    }

    /// <summary>
    /// 医療機関識別ID
    /// </summary>
    public int HpId
    {
        get { return KarteInf.HpId; }
    }

    /// <summary>
    /// 患者ID
    /// 患者を識別するためのシステム固有の番号
    /// </summary>
    public long PtId
    {
        get { return KarteInf.PtId; }
    }

    /// <summary>
    /// 診療日
    /// yyyymmdd
    /// </summary>
    public int SinDate
    {
        get { return KarteInf.SinDate; }
    }

    /// <summary>
    /// 来院番号
    /// </summary>
    public long RaiinNo
    {
        get { return KarteInf.RaiinNo; }
    }

    /// <summary>
    /// カルテ区分
    /// KARTE_KBN_MST.KARTE_KBN
    /// </summary>
    public int KarteKbn
    {
        get { return KarteInf.KarteKbn; }
    }

    /// <summary>
    /// 連番
    /// </summary>
    public long SeqNo
    {
        get { return KarteInf.SeqNo; }
    }

    /// <summary>
    /// テキスト
    /// </summary>
    public string Text
    {
        get { return KarteInf.Text ?? string.Empty; }
    }

    /// <summary>
    /// リッチテキスト
    /// </summary>
    public byte[] RichText
    {
        get
        {
            return KarteInf.RichText ?? default!;
        }
    }

    /// <summary>
    /// 削除区分
    /// 1: 削除 2: 未確定削除
    /// </summary>
    public int IsDeleted
    {
        get { return KarteInf.IsDeleted; }
    }

}
