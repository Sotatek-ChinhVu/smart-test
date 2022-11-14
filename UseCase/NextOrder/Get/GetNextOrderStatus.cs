namespace UseCase.NextOrder.Get
{
    public enum GetNextOrderStatus : byte
    {
        InvalidRsvkrtNo = 0,
        Successed = 1,
        InvalidHpId = 2,
        InvalidPtId = 3,
        InvalidSinDate = 4,
        NoData = 5,
        Failed = 6
    }
}
