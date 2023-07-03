namespace UseCase.NextOrder.CheckNextOrdHaveOdr
{
    public enum CheckNextOrdHaveOdrStatus : byte
    {
        Successed = 1,
        InvalidHpId = 2,
        InvalidPtId = 3,
        InvalidSinDate = 4,
    }
}
