using Domain.Models.TimeZone;
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
        return holidayMst != null && holidayMst.HolidayKbn != 0;
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

    public TimeZoneDayInfModel? GetLatestTimeZoneDayInf(int hpId, int sinDate, int uketukeTime)
    {
        var result = NoTrackingDataContext.TimeZoneDayInfs.Where(item => item.HpId == hpId &&
                                                                                      item.SinDate == sinDate &&
                                                                                      uketukeTime >= item.StartTime &&
                                                                                      (item.EndTime == 0 ? uketukeTime <= 2400 : uketukeTime <= item.EndTime))
                                                                .OrderByDescending(item => item.Id)
                                                                .FirstOrDefault();
        if (result != null)
        {
            return new TimeZoneDayInfModel(
                result.HpId,
                result.Id,
                result.SinDate,
                result.StartTime,
                result.EndTime,
                result.TimeKbn
            );
        }

        return null;
    }

    public bool UpdateTimeZoneDayInf(int hpId, int userId, int sinDate, int currentTimeKbn, int uketukeTime)
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
                updateTimeZoneDayInf.EndTime = int.Parse(CIUtil.GetJapanDateTimeNow().ToString("HHmm"));
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

    public List<TimeZoneConfGroupModel> GetTimeZoneConfGroupModels(int hpId)
    {
        var timeZoneConfEntities = NoTrackingDataContext.TimeZoneConfs.Where(x => x.HpId == hpId && x.IsDelete == 0).OrderBy(x => x.YoubiKbn);

        var result = timeZoneConfEntities
                    .GroupBy(x => x.YoubiKbn)
                    .AsEnumerable()
                    .Select(x => new TimeZoneConfGroupModel(
                        x.Key,
                        x.OrderBy((o) => o.StartTime).Select((detail, index) => new TimeZoneConfModel(detail.HpId,
                                                                                                            index + 1,
                                                                                                            detail.YoubiKbn,
                                                                                                            detail.StartTime,
                                                                                                            detail.EndTime,
                                                                                                            detail.SeqNo,
                                                                                                            detail.TimeKbn,
                                                                                                            detail.IsDelete,
                                                                                                            false))
                    )).ToList();

        List<int> dayOfWeek = new List<int> { 1, 2, 3, 4, 5, 6, 7 };

        foreach (var day in dayOfWeek)
        {
            var group = result.FirstOrDefault(x => x.YoubiKbn == day);
            if (group == null)
            {
                result.Add(
                    new TimeZoneConfGroupModel(
                        day,
                        new List<TimeZoneConfModel>() { }
                ));
            }
        }
        return result;
    }

    public bool SaveTimeZoneConf(int hpId, int userId, List<TimeZoneConfModel> timeZoneConfs)
    {
        List<TimeZoneConfModel> source = timeZoneConfs.Where(u => !u.CheckDefaultValue() && u.ModelModified).ToList();

        foreach (var model in source)
        {
            if (!model.CheckDefaultValue() && model.ModelModified)
            {
                if (model.SeqNo == 0 && model.IsDelete == 0)
                {
                    TrackingDataContext.TimeZoneConfs.Add(new TimeZoneConf()
                    {
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        CreateId = userId,
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateId = userId,
                        IsDelete = model.IsDelete,
                        EndTime = model.EndTime,
                        StartTime = model.StartTime,
                        HpId = hpId,
                        TimeKbn = model.TimeKbn,
                        YoubiKbn = model.YoubiKbn
                    });
                }
                else
                {
                    var update = TrackingDataContext.TimeZoneConfs.FirstOrDefault(x => x.HpId == hpId && x.YoubiKbn == model.YoubiKbn && x.SeqNo == model.SeqNo);
                    if (update != null)
                    {
                        update.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        update.UpdateId = userId;
                        update.EndTime = model.EndTime;
                        update.StartTime = model.StartTime;
                        update.IsDelete = model.IsDelete;
                        update.TimeKbn = model.TimeKbn;
                    }
                }
            }
        }
        return TrackingDataContext.SaveChanges() > 0;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
