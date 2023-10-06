using Helper.Extension;

namespace Domain.Models.Receipt;

public class RaiinInfModel
{
    public RaiinInfModel(long ptId, int sinDate, long raiinNo, string uketukeTime, string sinEndTime, int status)
    {
        PtId = ptId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        UketukeTime = uketukeTime;
        SinEndTime = sinEndTime;
        Status = status;
    }

    public long PtId { get; private set; }

    /// <summary>
    /// 診療日
    ///     yyyymmdd 
    /// </summary>
    public int SinDate { get; private set; }

    /// <summary>
    /// 来院番号
    /// </summary>
    public long RaiinNo { get; private set; }

    /// <summary>
    /// 受付時間
    ///     HH24MISS
    /// </summary>
    public string UketukeTime { get; private set; }

    /// <summary>
    /// 診察終了時間
    ///     HH24MISS　※状態が計算以上になった時間
    /// </summary>
    public string SinEndTime { get; private set; }


    /// <summary>
    /// 状態
    ///		0:予約
    ///		1:受付
    ///		3:一時保存
    ///		5:計算
    ///		7:精算待ち
    ///		9:精算済
    /// </summary>
    public int Status { get; private set; }
}
