namespace UseCase.MstItem.GetCmtCheckMstList
{
    public enum GetCmtCheckMstListStatus : byte
    {
        Successed = 1,
        InValidHpId = 2,
        InValidUserId = 3,
        InvalidItemCd = 4,
        Failed = 5
    }
}
