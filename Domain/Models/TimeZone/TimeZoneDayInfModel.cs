namespace Domain.Models.TimeZone;

public class TimeZoneDayInfModel
{
    public TimeZoneDayInfModel(int hpId, long id, int sinDate, int startTime, int endTime, int timeKbn)
    {
        HpId = hpId;
        Id = id;
        SinDate = sinDate;
        StartTime = startTime;
        EndTime = endTime;
        TimeKbn = timeKbn;
    }

    public TimeZoneDayInfModel()
    {
        HpId = 0;
        Id = 0;
        SinDate = 0;
        StartTime = 0;
        EndTime = 0;
        TimeKbn = 0;
    }

    public int HpId { get; private set; }

    public long Id { get; private set; }

    public int SinDate { get; private set; }

    public int StartTime { get; private set; }

    public int EndTime { get; private set; }

    public int TimeKbn { get; private set; }
}
