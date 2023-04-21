using Entity.Tenant;

namespace Reporting.Accounting.Model;

public class CoRsvInfModel 
{
    public RsvInf RsvInf { get; }

    public CoRsvInfModel(RsvInf rsvInf)
    {
        RsvInf = rsvInf;
    }

    /// <summary>
    /// 医療機関識別ID
    /// 
    /// </summary>
    public int HpId
    {
        get { return RsvInf.HpId; }
    }

    /// <summary>
    /// 予約枠ID
    /// 
    /// </summary>
    public int RsvFrameId
    {
        get { return RsvInf.RsvFrameId; }
    }

    /// <summary>
    /// 診療日
    /// 
    /// </summary>
    public int SinDate
    {
        get { return RsvInf.SinDate; }
    }

    /// <summary>
    /// 開始時間
    /// 
    /// </summary>
    public int StartTime
    {
        get { return RsvInf.StartTime; }
    }

    /// <summary>
    /// 予約番号
    /// 
    /// </summary>
    public long RaiinNo
    {
        get { return RsvInf.RaiinNo; }
    }

    /// <summary>
    /// 患者ID
    /// 
    /// </summary>
    public long PtId
    {
        get { return RsvInf.PtId; }
    }

    /// <summary>
    /// 予約種別コード
    /// 
    /// </summary>
    public int RsvSbt
    {
        get { return RsvInf.RsvSbt; }
    }

    /// <summary>
    /// 担当医師コード
    /// 
    /// </summary>
    public int TantoId
    {
        get { return RsvInf.TantoId; }
    }

    /// <summary>
    /// 診療科コード
    /// 
    /// </summary>
    public int KaId
    {
        get { return RsvInf.KaId; }
    }
}
