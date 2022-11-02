namespace UseCase.Reception.UpdateTimeZoneDayInf;

public enum UpdateTimeZoneDayInfStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidHpId = 3,
    InvalidUserId = 4,
    InvalidSinDate = 5,
    InvalidCurrentTimeKbn = 6,
    InvalidBeforeTimeKbn = 7,
    InvalidUketukeTime = 8,
    CanNotUpdateTimeZoneInf = 9
}
