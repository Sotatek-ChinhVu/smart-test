namespace UseCase.Accounting.SaveAccounting
{
    public enum SaveAccountingStatus
    {
        Success = 1,
        Failed = 2,
        InputDataNull = 3,
        InvalidHpId = 4,
        InvalidPtId = 5,
        InvalidUserId = 6,
        ValidateSuccess = 7,
        InvalidSumAdjust = 8,
        InvalidThisWari = 9,
        InvalidCredit = 10,
        InvalidPayType = 11,
        InvalidComment = 12,
        InvalidSindate = 13,
        InvalidRaiinNo = 14
    }
}
