namespace UseCase.AccountDue.SaveAccountDueList;

public enum SaveAccountDueListStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidHpId = 3,
    InvalidPtId = 4,
    InvalidSindate = 5,
    ValidateSuccess = 6,
    InvalidUserId = 7,
    InvalidNyukinKbn = 8,
    InvalidRaiinNo = 9,
    InvalidSortNo = 10,
    InvalidAdjustFutan = 11,
    InvalidNyukinGaku = 12,
    InvalidPaymentMethodCd = 13,
    InvalidNyukinDate = 14,
    InvalidUketukeSbt = 15,
    NyukinCmtMaxLength100 = 16,
    InvalidSeikyuGaku = 17,
    InvalidSeikyuTensu = 18,
    InvalidSeqNo = 19,
    NoItemChange = 20,
    InvalidSeikyuAdjustFutan = 21,
}
