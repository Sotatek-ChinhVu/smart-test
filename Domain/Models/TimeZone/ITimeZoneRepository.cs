namespace Domain.Models.TimeZone;

public interface ITimeZoneRepository
{
    DefaultSelectedTimeModel GetDefaultSelectedTime(int hpId, int sinDate, int birthDay);
}
