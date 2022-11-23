namespace UseCase.NextOrder.Upsert
{
    public enum UpsertNextOrderStatus : byte
    {
        Successed = 1,
        InvalidHpId = 2,
        InvalidPtId = 3,
        InvalidUserId = 4,
        Failed = 5
    }
}
