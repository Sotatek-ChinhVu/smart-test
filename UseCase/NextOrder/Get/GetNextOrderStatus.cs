namespace UseCase.NextOrder.Get
{
    public enum GetNextOrderStatus : byte
    {
        InvalidRsvkrtNo = 0,
        Successed = 1,
        InvalidHpId = 2,
        InvalidPtId = 3,
        InvalidSinDate = 4,
        InvalidUserId = 5,
        NoData = 6,
        Failed = 7
    }
}
