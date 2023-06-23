namespace UseCase.NextOrder.GetList
{
    public enum GetNextOrderListStatus : byte
    {
        Successed = 1,
        InvalidHpId = 2,
        InvalidPtId = 3,
        NoData = 4,
        Failed = 5
    }
}
