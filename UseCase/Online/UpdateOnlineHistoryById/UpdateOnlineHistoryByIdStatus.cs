namespace UseCase.Online.UpdateOnlineHistoryById;

public enum UpdateOnlineHistoryByIdStatus : byte
{
    ValidateSuccessed = 0,
    Successed = 1,
    Failed = 2,
    InvalidId = 3,
    InvalidPtId = 4,
    InvalidUketukeStatus = 5,
}
