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
        InvalidPayType = 10,
        InvalidComment = 11,
        InvalidSindate = 12,
        InvalidRaiinNo = 13,
        NoPermission = 14,
    }
}
