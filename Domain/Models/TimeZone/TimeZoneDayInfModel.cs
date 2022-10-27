namespace Domain.Models.TimeZoneDayInf;

public class TimeZoneDayInfModel
{
    public TimeZoneDayInfModel()
    {
        HpId = 0;
        SinDate = 0;
        TimeKbn = 0;
        StartTime = 0;
        EndTime = 0;
    }
    public TimeZoneDayInfModel(int hpId, int sinDate, int timeKbn, int startTime, int endTime)
    {
        HpId = hpId;
        SinDate = sinDate;
        TimeKbn = timeKbn;
        StartTime = startTime;
        EndTime = endTime;
    }

    public int HpId { get; private set; }
    public int SinDate { get; private set; }
    public int TimeKbn { get; private set; }
    public int StartTime { get; private set; }
    public int EndTime { get; private set; }
}
