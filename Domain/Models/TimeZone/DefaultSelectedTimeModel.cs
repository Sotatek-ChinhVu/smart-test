namespace Domain.Models.TimeZone;

public class DefaultSelectedTimeModel
{
    public DefaultSelectedTimeModel()
    {
        TimeKbnName = string.Empty;
        UketukeTime = 0;
        StartTime = string.Empty;
        EndTime = string.Empty;
        IsShowPopup = false;
        JikanKbnDefault = 0;
        CurrentTimeKbn = 0;
        BeforeTimeKbn = 0;
    }

    public DefaultSelectedTimeModel(string timeKbnName, int uketukeTime, string startTime, string endTime, int currentTimeKbn, int beforeTimeKbn, bool isShowPopup, int jikanKbnDefault)
    {
        TimeKbnName = timeKbnName;
        UketukeTime = uketukeTime;
        StartTime = startTime;
        EndTime = endTime;
        CurrentTimeKbn = currentTimeKbn;
        BeforeTimeKbn = beforeTimeKbn;
        IsShowPopup = isShowPopup;
        JikanKbnDefault = jikanKbnDefault;
    }




    // for message popup
    public string TimeKbnName { get; private set; }

    public int UketukeTime { get; private set; }

    public string StartTime { get; private set; }

    public string EndTime { get; private set; }

    public int CurrentTimeKbn { get; private set; }

    public int BeforeTimeKbn { get; private set; }

    public bool IsShowPopup { get; private set; }


    // for default value JikanKbn
    public int JikanKbnDefault { get; private set; }
}
