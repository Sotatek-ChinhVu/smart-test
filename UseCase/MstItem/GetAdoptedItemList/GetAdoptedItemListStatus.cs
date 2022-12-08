namespace UseCase.MstItem.GetAdoptedItemList
{
    public enum GetAdoptedItemListStatus : byte
    {
        Successed = 1,
        InValidHpId = 2,
        InvalidSindate = 3,
        InvalidItemCds = 4,
        Failed = 5
    }
}
