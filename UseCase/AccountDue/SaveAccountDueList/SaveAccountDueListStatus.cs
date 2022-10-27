namespace UseCase.AccountDue.SaveAccountDueList;

public enum SaveAccountDueListStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidHpId = 3,
    InvalidPtId = 4,
    InvalidSindate = 5,
    InvalidpageSize = 6,
    InvalidpageIndex = 7,
}
