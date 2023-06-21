using Helper.Common;
using UseCase.Core.Sync.Core;
using static Helper.Common.UserConfCommon;

namespace UseCase.MedicalExamination.GetHeaderVistitDate;

public class GetHeaderVistitDateOutputData : IOutputData
{
    public GetHeaderVistitDateOutputData(DateTimeFormart dateTimeFormart, int firstDate, int lastDate, GetHeaderVistitDateStatus status)
    {
        switch (dateTimeFormart)
        {
            case DateTimeFormart.JapaneseCalendar:
                FirstDate = CIUtil.SDateToShowWDate2(firstDate);
                LastDate = CIUtil.SDateToShowWDate2(lastDate);
                break;
            case DateTimeFormart.WesternCalendar:
                FirstDate = CIUtil.SDateToShowSWDate(firstDate, -1, 0, 1);
                LastDate = CIUtil.SDateToShowSWDate(lastDate, -1, 0, 1);
                break;
            case DateTimeFormart.JapAndWestCalendar:
                FirstDate = CIUtil.SDateToShowWSDate(firstDate);
                LastDate = CIUtil.SDateToShowWSDate(lastDate);
                break;
            default:
                FirstDate = string.Empty;
                LastDate = string.Empty;
                break;
        }
        Status = status;
    }

    public string FirstDate { get; private set; }

    public string LastDate { get; private set; }

    public GetHeaderVistitDateStatus Status { get; private set; }
}
