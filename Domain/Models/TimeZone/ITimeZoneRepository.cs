namespace Domain.Models.TimeZone;

public interface ITimeZoneRepository
{
    public int GetTimeKbnForChild(bool isPatientChildren, int dayOfWeek, int uketukeTime);

    public bool IsPatientChildren(int hpId, int birthDay, int sinDate);

    public bool IsHoliday(int hpId, int sinDate);

    public int GetShonikaSetting(int hpId, int presentDate);

    public List<TimeZoneConfModel> GetTimeZoneConfs(int hpId);

    public TimeZoneDayInfModel GetLatestTimeZoneDayInf(int hpId, int sinDate, int uketukeTime);
}
