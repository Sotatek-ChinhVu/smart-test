namespace Reporting.OrderLabel.Model;

public class CoYoyakuModel
{
    public CoYoyakuModel(int sinDate, string time, string frame)
    {
        SinDate = sinDate;
        Time = time;
        Frame = frame;
    }

    /// <summary>
    /// 予約日
    /// </summary>
    public int SinDate { get; set; }
    /// <summary>
    /// 予約時間
    /// </summary>
    public string Time { get; set; }
    /// <summary>
    /// 予約枠名
    /// </summary>
    public string Frame { get; set; }
}
