namespace UseCase.Reception.GetDefaultSelectedTime;

public enum GetDefaultSelectedTimeStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidHpId = 3,
    InvalidSinDate = 4,
    InvalidBirthDay = 5
}
