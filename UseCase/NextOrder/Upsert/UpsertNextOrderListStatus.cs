namespace UseCase.NextOrder.Upsert
{
    public enum UpsertNextOrderListStatus : byte
    {
        Successed = 1,
        InvalidHpId = 2,
        InvalidPtId = 3,
        InvalidUserId = 4,
        InvalidRsvkrtNo = 5,
        InvalidRsvkrtDate = 6,
        Failed = 7
    }
}
