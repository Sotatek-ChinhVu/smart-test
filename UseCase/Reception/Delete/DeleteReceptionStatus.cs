namespace UseCase.Reception.Delete
{
    public enum DeleteReceptionStatus : byte
    {
        InvalidRaiinNo = 0,
        Successed = 1,
        InvalidHpId = 2,
        InvalidUserId = 3,
        Failed = 4,
    }
}
