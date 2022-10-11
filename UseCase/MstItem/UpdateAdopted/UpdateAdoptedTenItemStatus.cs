namespace UseCase.MstItem.UpdateAdopted
{
    public enum UpdateAdoptedTenItemStatus: byte
    {
        Successed = 1,
        InvalidValueAdopted = 2,
        InvalidItemCd = 3,
        InvalidStartDate = 4,
        Failed = 5,
    }
}
