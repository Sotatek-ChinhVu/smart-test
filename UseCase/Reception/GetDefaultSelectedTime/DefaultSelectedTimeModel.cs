﻿namespace UseCase.Reception.GetDefaultSelectedTime;

public class DefaultSelectedTimeModel
{
    public DefaultSelectedTimeModel()
    {
        TimeKbnName = string.Empty;
        UketukeTime = string.Empty;
        StartTime = string.Empty;
        EndTime = string.Empty;
        IsShowPopup = false;
        JikanKbnDefault = 0;
        CurrentTimeKbn = 0;
        BeforeTimeKbn = 0;
    }

    public DefaultSelectedTimeModel(string timeKbnName, string uketukeTime, string startTime, string endTime, int currentTimeKbn, int beforeTimeKbn, bool isPatientChildren, bool isShowPopup, int jikanKbnDefault)
    {
        TimeKbnName = timeKbnName;
        UketukeTime = uketukeTime;
        StartTime = startTime;
        EndTime = endTime;
        CurrentTimeKbn = currentTimeKbn;
        BeforeTimeKbn = beforeTimeKbn;
        IsShowPopup = isShowPopup;
        JikanKbnDefault = jikanKbnDefault;
        IsPatientChildren = isPatientChildren;
    }

    // for message popup
    public string TimeKbnName { get; private set; }

    public string UketukeTime { get; private set; }

    public string StartTime { get; private set; }

    public string EndTime { get; private set; }

    public int CurrentTimeKbn { get; private set; }

    public int BeforeTimeKbn { get; private set; }

    public bool IsPatientChildren { get; private set; }

    public bool IsShowPopup { get; private set; }


    // for default value JikanKbn
    public int JikanKbnDefault { get; private set; }
}
