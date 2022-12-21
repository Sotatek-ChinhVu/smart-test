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
            var resultValidate = ValidateInput(inputData);
            if (resultValidate != GetDefaultSelectedTimeStatus.ValidateSuccess)
            {
                return new GetDefaultSelectedTimeOutputData(resultValidate);
            }
            var result = GetDefaultSelectedTime(inputData.HpId, inputData.UketukeTime, inputData.SinDate, inputData.BirthDay);
            return new GetDefaultSelectedTimeOutputData(GetDefaultSelectedTimeStatus.Successed, result);
        }
        catch
        {
            return new GetDefaultSelectedTimeOutputData(GetDefaultSelectedTimeStatus.Failed);
        }
    }

    private GetDefaultSelectedTimeStatus ValidateInput(GetDefaultSelectedTimeInputData inputData)
    {
        var uketukeTime = inputData.UketukeTime.ToString().PadLeft(4, '0').ToString();
        var HH = int.Parse(uketukeTime.Substring(0, 2));
        var ss = int.Parse(uketukeTime.Substring(2, 2));
        if (uketukeTime.Length > 4
            || HH < 0 || HH > 24
            || ss < 0 || ss > 60)
        {
            return GetDefaultSelectedTimeStatus.InvalidUketukeTime;
        }
        else if (inputData.SinDate.ToString().Length != 8)
        {
            return GetDefaultSelectedTimeStatus.InvalidSinDate;
        }
        else if (inputData.BirthDay.ToString().Length != 8)
        {
            return GetDefaultSelectedTimeStatus.InvalidBirthDay;
        }
        return GetDefaultSelectedTimeStatus.ValidateSuccess;
    }

    private DefaultSelectedTimeModel GetDefaultSelectedTime(int hpId, int uketukeTime, int sinDate, int birthDay)
    {
        int dayOfWeek = CIUtil.DayOfWeek(CIUtil.IntToDate(sinDate));
        string startTime = string.Empty, endTime = string.Empty;
        int currentTimeKbn = 0, beforeTimeKbn = 0;
        bool isShowPopup = false;
        bool isPatientChildren = IsPatientChildren(hpId, birthDay, sinDate);
        bool isHoliday = _timeZoneRepository.IsHoliday(hpId, sinDate);

        //Child Patient
        int timeKbnForChild = GetTimeKbnForChild(isPatientChildren, dayOfWeek, uketukeTime);

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
                        CIUtil.TimeToShowTime(uketukeTime),
                        startTime,
                        endTime,
                        currentTimeKbn,
                        beforeTimeKbn,
                        isPatientChildren,
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

    private int GetTimeKbnForChild(bool isPatientChildren, int dayOfWeek, int uketukeTime)
    {
        int timeKbnForChild = 0;
        if (isPatientChildren)
        {
            //夜間小特 : 6h-8h or 18h-22h
            if ((uketukeTime >= 600 && uketukeTime < 800) ||
                ((dayOfWeek == 7 ? uketukeTime >= 1200 : uketukeTime >= 1800) && uketukeTime < 2200))
            {
                timeKbnForChild = JikanConst.YakanKotoku;
            }
            //深夜小特 : 22h-6h
            else if (uketukeTime >= 2200 || uketukeTime < 600)
            {
                timeKbnForChild = JikanConst.SinyaKotoku;
            }
        }
        return timeKbnForChild;
    }

    private bool IsPatientChildren(int hpId, int birthDay, int sinDate)
    {
        if (_timeZoneRepository.GetShonikaSetting(hpId, sinDate) == 1)
        {
            int age = CIUtil.SDateToAge(birthDay, sinDate);
            if (age >= 0 && age < 6)
            {
                return true;
            }
        }
        return false;
    }
}
