namespace UseCase.Reception.GetLastRaiinInfs;

public enum GetLastRaiinInfsStatus : byte
{
    Successed = 0,
    InvalidHpId = 1,
    InvalidPtId = 2,
    InvalidSinDate = 3,
    Failed = 4
}