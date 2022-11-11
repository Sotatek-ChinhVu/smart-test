namespace UseCase.SuperSetDetail.GetSuperSetDetailToDoTodayOrder;

public enum GetSuperSetDetailToDoTodayOrderStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidHpId = 3,
    InvalidSetCd = 4,
    InvalidSinDate = 5,
    NoData = 6
}
