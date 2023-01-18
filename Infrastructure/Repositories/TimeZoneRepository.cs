﻿using Domain.Models.TimeZone;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class TimeZoneRepository : RepositoryBase, ITimeZoneRepository
{
    public TimeZoneRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public bool IsHoliday(int hpId, int sinDate)
    {
        var holidayMst = NoTrackingDataContext.HolidayMsts.FirstOrDefault(item => item.HpId == hpId && item.SinDate == sinDate && item.IsDeleted != 1);
        return holidayMst != null && holidayMst.HolidayKbn != 0 && holidayMst.KyusinKbn == 1;
    }

    public int GetShonikaSetting(int hpId, int presentDate)
    {
        var systemConfig = NoTrackingDataContext.SystemGenerationConfs.FirstOrDefault(
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
        return NoTrackingDataContext.TimeZoneConfs.Where(item => item.HpId == hpId && item.IsDelete == DeleteTypes.None)
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
        var result = NoTrackingDataContext.TimeZoneDayInfs.Where(item => item.HpId == hpId &&
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

    public bool UpdateTimeZoneDayInf(int hpId, int userId, int sinDate, int currentTimeKbn, int uketukeTime)
    {
        try
        {
            //update latest timeZoneDayInf in sindate
            var updateTimeZoneDayInf = TrackingDataContext.TimeZoneDayInfs.Where(t => t.HpId == hpId &&
                                                                t.SinDate == sinDate &&
                                                                uketukeTime >= t.StartTime &&
                                                                (t.EndTime == 0 ? uketukeTime <= 2400 : uketukeTime <= t.EndTime))
                                                                .OrderByDescending(t => t.Id)
                                                                .FirstOrDefault();
            if (updateTimeZoneDayInf != null)
            {
                updateTimeZoneDayInf.EndTime = int.Parse(DateTime.Now.ToString("HHmm"));
            }

            //then add new timeZoneDayInf
            TimeZoneDayInf timeDayInf = new TimeZoneDayInf()
            {
                HpId = hpId,
                SinDate = sinDate,
                TimeKbn = currentTimeKbn,
                StartTime = uketukeTime,
                EndTime = 0,
                CreateId = userId,
                UpdateId = userId,
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateDate = CIUtil.GetJapanDateTimeNow()
            };
            TrackingDataContext.TimeZoneDayInfs.Add(timeDayInf);

            return TrackingDataContext.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
