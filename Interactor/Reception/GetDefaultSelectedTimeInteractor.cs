using Domain.Models.TimeZone;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
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
        finally
        {
            _timeZoneRepository.ReleaseResource();
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
        string timeKbnName = string.Empty;
        //Child Patient
        int jikanKbn = 0;
        int timeKbnForChild = 0;
        var timeZoneConfs = _timeZoneRepository.GetTimeZoneConfs(hpId);
        TimeZoneConfModel? timeZoneConf = null;
        if (timeZoneConfs != null && timeZoneConfs.Any())
        {
            timeZoneConf = timeZoneConfs.Find(t => t.YoubiKbn == dayOfWeek && t.StartTime <= uketukeTime && t.EndTime > uketukeTime);
        }
        if (isPatientChildren)
        {
            if ((isHoliday || dayOfWeek == 1) && uketukeTime >= 600 && uketukeTime < 2200)
            {
                timeKbnForChild = JikanConst.KyujituKotoku;
            }
            //夜間小特 : 6h-8h or 18h-22h
            else if ((uketukeTime >= 600 && uketukeTime < 800) ||
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

        //Adult Patient

        if (isHoliday)
        {
            if (timeZoneConf == null)
            {
                jikanKbn = JikanConst.Kyujitu;
                return new DefaultSelectedTimeModel(
                    timeKbnName,
                    CIUtil.TimeToShowTime(uketukeTime),
                    startTime,
                    endTime,
                    currentTimeKbn,
                    beforeTimeKbn,
                    isPatientChildren,
                    isShowPopup,
                    jikanKbn,
                    timeKbnForChild);

            }
        }
        if (sinDate != CIUtil.GetJapanDateTimeNow().ToString("yyyyMMdd").AsInteger())
        {
            jikanKbn = JikanConst.JikanNai;
            return new DefaultSelectedTimeModel(
                    timeKbnName,
                    CIUtil.TimeToShowTime(uketukeTime),
                    startTime,
                    endTime,
                    currentTimeKbn,
                    beforeTimeKbn,
                    isPatientChildren,
                    isShowPopup,
                    jikanKbn,
                    timeKbnForChild);
        }
        if (timeZoneConfs == null)
        {
            jikanKbn = JikanConst.JikanNai;
            return new DefaultSelectedTimeModel(
                    timeKbnName,
                    CIUtil.TimeToShowTime(uketukeTime),
                    startTime,
                    endTime,
                    currentTimeKbn,
                    beforeTimeKbn,
                    isPatientChildren,
                    isShowPopup,
                    jikanKbn,
                    timeKbnForChild);
        }

        if (timeZoneConf != null)
        {
            currentTimeKbn = timeZoneConf.TimeKbn;
        }
        var timeZoneDayInf = _timeZoneRepository.GetLatestTimeZoneDayInf(hpId, sinDate, uketukeTime);
        if (timeZoneDayInf != null)
        {
            beforeTimeKbn = timeZoneDayInf.TimeKbn;
        }
        if (currentTimeKbn != beforeTimeKbn)
        {
            timeKbnName = JikanConst.JikanKotokuDict[currentTimeKbn];

            //change out of time => in of time
            if (currentTimeKbn == JikanConst.JikanNai)
            {
                isShowPopup = true;
            }
            //change in of time => out of time
            else
            {

                startTime = CIUtil.TimeToShowTime(timeZoneConf?.StartTime ?? 0);
                endTime = CIUtil.TimeToShowTime(timeZoneConf?.EndTime ?? 0);
                isShowPopup = true;
            }

        }

        if (currentTimeKbn == 0)
        {
            jikanKbn = currentTimeKbn;

            return new DefaultSelectedTimeModel(
                        timeKbnName,
                        CIUtil.TimeToShowTime(uketukeTime),
                        startTime,
                        endTime,
                        currentTimeKbn,
                        beforeTimeKbn,
                        isPatientChildren,
                        isShowPopup,
                        jikanKbn,
                        timeKbnForChild);
        };
        if (timeKbnForChild > 0 &&
          (timeKbnForChild == JikanConst.YakanKotoku && currentTimeKbn == JikanConst.Yasou) ||
          (timeKbnForChild == JikanConst.SinyaKotoku && currentTimeKbn == JikanConst.Sinya))
        {
            jikanKbn = timeKbnForChild;

            return new DefaultSelectedTimeModel(
                        timeKbnName,
                        CIUtil.TimeToShowTime(uketukeTime),
                        startTime,
                        endTime,
                        currentTimeKbn,
                        beforeTimeKbn,
                        isPatientChildren,
                        isShowPopup,
                        jikanKbn,
                        timeKbnForChild);
        }

        jikanKbn = currentTimeKbn;

        return new DefaultSelectedTimeModel(
                        timeKbnName,
                        CIUtil.TimeToShowTime(uketukeTime),
                        startTime,
                        endTime,
                        currentTimeKbn,
                        beforeTimeKbn,
                        isPatientChildren,
                        isShowPopup,
                        jikanKbn,
                        timeKbnForChild);

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
