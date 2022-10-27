using Domain.Models.TimeZone;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class TimeZoneRepository : ITimeZoneRepository
{
    private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
    private readonly TenantDataContext _tenantDataContext;
    public TimeZoneRepository(ITenantProvider tenantProvider)
    {
        _tenantNoTrackingDataContext = tenantProvider.GetNoTrackingDataContext();
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }
    public DefaultSelectedTimeModel GetDefaultSelectedTime(int hpId, int sinDate, int birthDay)
    {
        int dayOfWeek = CIUtil.DayOfWeek(CIUtil.IntToDate(sinDate));
        int uketukeTime = int.Parse(DateTime.Now.ToString("HHmm"));
        string startTime = string.Empty, endTime = string.Empty;
        int currentTimeKbn = 0, beforeTimeKbn = 0;
        bool isShowPopup = false;
        bool isPatientChildren = IsPatientChildren(hpId, birthDay, sinDate);
        bool isHoliday = IsHoliday(hpId, sinDate);

        //Child Patient
        int timeKbnForChild = GetTimeKbnForChild(isPatientChildren, dayOfWeek, uketukeTime);

        //Adult Patient
        var listTimeZoneConfig = GetTimeZoneConfs(hpId);


        var timeZoneConfig = listTimeZoneConfig.FirstOrDefault(item => item.YoubiKbn == dayOfWeek && item.StartTime <= uketukeTime && item.EndTime > uketukeTime);
        if (timeZoneConfig != null)
        {
            currentTimeKbn = timeZoneConfig.TimeKbn;
        }

        var timeZoneDayInf = GetLatestTimeZoneDayInf(hpId, sinDate, uketukeTime);
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
                        isShowPopup,
                        GetJikanKbnDefaultValue(isPatientChildren, isHoliday, uketukeTime, sinDate, currentTimeKbn, timeKbnForChild, listTimeZoneConfig)
                    );
    }

    private int GetJikanKbnDefaultValue(bool isPatientChildren, bool isHoliday, int uketukeTime, int sinDate, int currentTimeKbn, int timeKbnForChild, List<TimeZoneConf> listTimeZoneConfig)
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
        if (GetShonikaSetting(hpId, sinDate) == 1)
        {
            int age = CIUtil.SDateToAge(birthDay, sinDate);
            if (age >= 0 && age < 6)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsHoliday(int hpId, int sinDate)
    {
        var holidayMst = _tenantNoTrackingDataContext.HolidayMsts.FirstOrDefault(item => item.HpId == hpId && item.SinDate == sinDate && item.IsDeleted != 1);
        return holidayMst != null && holidayMst.HolidayKbn != 0 && holidayMst.KyusinKbn == 1;
    }

    private int GetShonikaSetting(int hpId, int presentDate)
    {
        var systemConfig = _tenantNoTrackingDataContext.SystemGenerationConfs.FirstOrDefault(
                                item => item.HpId == hpId
                                && item.GrpCd == 8001
                                && item.GrpEdaNo == 0
                                && item.StartDate <= presentDate
                                && item.EndDate >= presentDate
                            );
        if (systemConfig != null)
        {
            return systemConfig.Val;
        }
        return 0;
    }

    private List<TimeZoneConf> GetTimeZoneConfs(int hpId)
    {
        return _tenantNoTrackingDataContext.TimeZoneConfs.Where(t => t.HpId == hpId && t.IsDelete == DeleteTypes.None).ToList();
    }

    private TimeZoneDayInf GetLatestTimeZoneDayInf(int hpId, int sinDate, int uketukeTime)
    {
        var result = _tenantNoTrackingDataContext.TimeZoneDayInfs.Where(item => item.HpId == hpId &&
                                                                                      item.SinDate == sinDate &&
                                                                                      uketukeTime >= item.StartTime &&
                                                                                      (item.EndTime == 0 ? uketukeTime <= 2400 : uketukeTime <= item.EndTime))
                                                                .OrderByDescending(item => item.Id)
                                                                .FirstOrDefault();
        return result != null ? result : new TimeZoneDayInf();
    }
}
