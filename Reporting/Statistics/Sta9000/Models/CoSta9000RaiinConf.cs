namespace Reporting.Statistics.Sta9000.Models;

public class CoSta9000RaiinConf
{
    public CoSta9000RaiinConf()
    {
        AgeFrom = -1;
        AgeTo = -1;
        Statuses = new();
        UketukeSbts = new();
        KaIds = new();
        TantoIds = new();
        JikanKbns = new();
    }

    /// <summary>
    /// 来院日Start
    /// </summary>
    public int StartSinDate { get; set; }

    /// <summary>
    /// 来院日End
    /// </summary>
    public int EndSinDate { get; set; }

    /// <summary>
    /// 状態
    ///     0:予約 1:受付 3:一時保存 5:計算 7:精算待ち 9:済み
    /// </summary>
    public List<int> Statuses { get; set; }

    /// <summary>
    /// 受付種別
    /// </summary>
    public List<int> UketukeSbts { get; set; }

    /// <summary>
    /// 診療科ID
    /// </summary>
    public List<int> KaIds { get; set; }

    /// <summary>
    /// 担当医ID
    /// </summary>
    public List<int> TantoIds { get; set; }

    /// <summary>
    /// 最終来院日Start
    /// </summary>
    public int StartLastVisitDate { get; set; }

    /// <summary>
    /// 最終来院日End
    /// </summary>
    public int EndLastVisitDate { get; set; }

    /// <summary>
    /// 新患
    ///      1:新規患者のみ
    /// </summary>
    public int IsSinkan { get; set; }

    /// <summary>
    /// 来院日時点の年齢From
    /// </summary>
    public int AgeFrom { get; set; }

    /// <summary>
    /// 来院日時点の年齢To
    /// </summary>
    public int AgeTo { get; set; }

    /// <summary>
    /// 時間枠区分
    ///     1:時間内 2:時間外 3:休日 4:深夜 5:夜・早
    /// </summary>
    public List<int> JikanKbns { get; set; }
}
