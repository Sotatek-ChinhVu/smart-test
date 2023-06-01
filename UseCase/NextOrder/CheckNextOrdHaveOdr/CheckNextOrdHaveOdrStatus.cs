namespace UseCase.NextOrder.CheckNextOrdHaveOdr
{
    public enum CheckNextOrdHaveOdrStatus : byte
    {
        Successed = 1,
        InvalidHpId = 2,
        InvalidPtId = 3,
        InvalidUserId = 4,
        InvalidRsvkrtNo = 5,
        InvalidRsvkrtDate = 6,
        Failed = 7
    }
}
