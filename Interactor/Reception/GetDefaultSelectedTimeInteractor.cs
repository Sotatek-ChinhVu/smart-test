using Domain.Models.TimeZone;
using Helper.Common;
using Helper.Constants;
using UseCase.Reception.GetDefaultSelectedTime;

namespace Interactor.Reception;

public class GetDefaultSelectedTimeInteractor : IGetDefaultSelectedTimeInputPort
{
    private readonly ITimeZoneRepository _timeZoneRepository;

    public GetDefaultSelectedTimeInteractor(ITimeZoneRepository timeZoneRepository)
    {
        _timeZoneRepository = timeZoneRepository;
    }

    public GetDefaultSelectedTimeOutputData Handle(GetDefaultSelectedTimeInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new GetDefaultSelectedTimeOutputData(GetDefaultSelectedTimeStatus.InvalidHpId);
            }
            else if (inputData.SinDate.ToString().Length != 8)
            {
                return new GetDefaultSelectedTimeOutputData(GetDefaultSelectedTimeStatus.InvalidSinDate);
            }
            else if (inputData.BirthDay.ToString().Length != 8)
            {
                return new GetDefaultSelectedTimeOutputData(GetDefaultSelectedTimeStatus.InvalidBirthDay);
            }
            var result = GetDefaultSelectedTime(inputData.HpId, inputData.SinDate, inputData.BirthDay);
            return new GetDefaultSelectedTimeOutputData(GetDefaultSelectedTimeStatus.Successed, result);
        }
        catch
        {
            return new GetDefaultSelectedTimeOutputData(GetDefaultSelectedTimeStatus.Failed);
        }
    }
    private DefaultSelectedTimeModel GetDefaultSelectedTime(int hpId, int sinDate, int birthDay)
    {
        int dayOfWeek = CIUtil.DayOfWeek(CIUtil.IntToDate(sinDate));
        int uketukeTime = int.Parse(DateTime.Now.ToString("HHmm"));
        string startTime = string.Empty, endTime = string.Empty;
        int currentTimeKbn = 0, beforeTimeKbn = 0;
        bool isShowPopup = false;
        bool isPatientChildren = _timeZoneRepository.IsPatientChildren(hpId, birthDay, sinDate);
        bool isHoliday = _timeZoneRepository.IsHoliday(hpId, sinDate);

        //Child Patient
        int timeKbnForChild = _timeZoneRepository.GetTimeKbnForChild(isPatientChildren, dayOfWeek, uketukeTime);

        //Adult Patient
        var listTimeZoneConfig = _timeZoneRepository.GetTimeZoneConfs(hpId);

        var timeZoneConfig = listTimeZoneConfig.FirstOrDefault(item => item.YoubiKbn == dayOfWeek && item.StartTime <= uketukeTime && item.EndTime > uketukeTime);
        if (timeZoneConfig != null)
        {
            currentTimeKbn = timeZoneConfig.TimeKbn;
        }

        var timeZoneDayInf = _timeZoneRepository.GetLatestTimeZoneDayInf(hpId, sinDate, uketukeTime);
        if (timeZoneDayInf != null)
        {
            beforeTimeKbn = timeZoneDayInf.TimeKbn;
        }

        string timeKbnName = string.Empty;
        if (currentTimeKbn != beforeTimeKbn)
        {
            //GetDataForMessage
            isShowPopup = true;
            timeKbnName = JikanConst.JikanKotokuDict[currentTimeKbn];
            if (currentTimeKbn != JikanConst.JikanNai && timeZoneConfig != null)
            {
                startTime = CIUtil.TimeToShowTime(timeZoneConfig.StartTime);
                endTime = CIUtil.TimeToShowTime(timeZoneConfig.EndTime);
            }
        }
        return new DefaultSelectedTimeModel(
                        timeKbnName,
                        uketukeTime,
                        startTime,
                        endTime,
                        currentTimeKbn,
                        beforeTimeKbn,
                        isShowPopup,
                        GetJikanKbnDefaultValue(isPatientChildren, isHoliday, uketukeTime, sinDate, currentTimeKbn, timeKbnForChild, listTimeZoneConfig)
                    );
    }

    private int GetJikanKbnDefaultValue(bool isPatientChildren, bool isHoliday, int uketukeTime, int sinDate, int currentTimeKbn, int timeKbnForChild, List<TimeZoneConfModel> listTimeZoneConfig)
    {
        if (isPatientChildren && isHoliday && uketukeTime >= 600 && uketukeTime < 2200)
        {
            return JikanConst.KyujituKotoku;
        }
        else if (isHoliday)
        {
            return JikanConst.Kyujitu;
        }
        else if (sinDate != int.Parse(DateTime.Now.ToString("yyyyMMdd")))
        {
            return JikanConst.JikanNai;
        }
        else if (!listTimeZoneConfig.Any())
        {
            return JikanConst.JikanNai;
        }
        else if (currentTimeKbn == 0)
        {
            return currentTimeKbn;
        }
        else if (timeKbnForChild > 0 &&
          (timeKbnForChild == JikanConst.YakanKotoku && currentTimeKbn == JikanConst.Yasou) ||
          (timeKbnForChild == JikanConst.SinyaKotoku && currentTimeKbn == JikanConst.Sinya))
        {
            return timeKbnForChild;
        }
        return currentTimeKbn;
    }
}
