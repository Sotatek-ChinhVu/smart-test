namespace UseCase.NextOrder.GetList
{
    public enum GetNextOrderListStatus : byte
    {
        Successed = 1,
        InvalidHpId = 2,
        InvalidPtId = 3,
        InvalidRsvkrtKbn = 4,
        NoData = 5,
        Failed = 6
    }
}
