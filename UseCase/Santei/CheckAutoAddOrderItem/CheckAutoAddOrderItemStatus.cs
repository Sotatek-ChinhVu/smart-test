namespace UseCase.Santei.CheckAutoAddOrderItem
{
    public enum CheckAutoAddOrderItemStatus : byte
    {
        Successed = 1,
        InvalidHpId = 2,
        InvalidSinDate = 3,
        InvalidItemCd = 4,
        NoData = 5,
    }
}
