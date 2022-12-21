using Domain.Common;

namespace Domain.Models.TimeZone;

public interface ITimeZoneRepository : IRepositoryBase
{
    public bool IsHoliday(int hpId, int sinDate);

    public int GetShonikaSetting(int hpId, int presentDate);

    public List<TimeZoneConfModel> GetTimeZoneConfs(int hpId);

    public TimeZoneDayInfModel GetLatestTimeZoneDayInf(int hpId, int sinDate, int uketukeTime);

    public bool UpdateTimeZoneDayInf(int hpId, int userId, int sinDate, int currentTimeKbn, int uketukeTime);
}
