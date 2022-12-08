namespace UseCase.MstItem.UpdateAdoptedItemList
{
    public enum UpdateAdoptedItemListStatus : byte
    {
        Successed = 1,
        InValidHpId = 2,
        InvalidSindate = 3,
        InvalidItemCds = 4,
        InvalidUserId = 5,
        InvalidValueAdopted = 6,
        Failed = 7
    }
}
