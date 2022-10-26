namespace UseCase.AccountDue.GetAccountDueList;

public enum GetAccountDueListStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidHpId = 3,
    InvalidPtId = 4,
    InvalidSindate = 5,
    InvalidpageSize = 6,
    InvalidpageIndex = 7,
}
