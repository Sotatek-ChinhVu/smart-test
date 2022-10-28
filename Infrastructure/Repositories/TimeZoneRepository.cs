using Domain.Models.TimeZone;
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
    public int GetTimeKbnForChild(bool isPatientChildren, int dayOfWeek, int uketukeTime)
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

    public bool IsPatientChildren(int hpId, int birthDay, int sinDate)
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

    public bool IsHoliday(int hpId, int sinDate)
    {
        var holidayMst = _tenantNoTrackingDataContext.HolidayMsts.FirstOrDefault(item => item.HpId == hpId && item.SinDate == sinDate && item.IsDeleted != 1);
        return holidayMst != null && holidayMst.HolidayKbn != 0 && holidayMst.KyusinKbn == 1;
    }

    public int GetShonikaSetting(int hpId, int presentDate)
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

    public List<TimeZoneConfModel> GetTimeZoneConfs(int hpId)
    {
        return _tenantNoTrackingDataContext.TimeZoneConfs.Where(item => item.HpId == hpId && item.IsDelete == DeleteTypes.None)
                                                         .Select(item => new TimeZoneConfModel(
                                                                item.HpId,
                                                                item.YoubiKbn,
                                                                item.SeqNo,
                                                                item.StartTime,
                                                                item.EndTime,
                                                                item.TimeKbn
                                                             ))
                                                         .ToList();
    }

    public TimeZoneDayInfModel GetLatestTimeZoneDayInf(int hpId, int sinDate, int uketukeTime)
    {
        var result = _tenantNoTrackingDataContext.TimeZoneDayInfs.Where(item => item.HpId == hpId &&
                                                                                      item.SinDate == sinDate &&
                                                                                      uketukeTime >= item.StartTime &&
                                                                                      (item.EndTime == 0 ? uketukeTime <= 2400 : uketukeTime <= item.EndTime))
                                                                .OrderByDescending(item => item.Id)
                                                                .FirstOrDefault();
        return new TimeZoneDayInfModel(
                result != null ? result.HpId : 0,
                result != null ? result.Id : 0,
                result != null ? result.SinDate : 0,
                result != null ? result.StartTime : 0,
                result != null ? result.EndTime : 0,
                result != null ? result.TimeKbn : 0
            );
    }
}
