namespace UseCase.OrdInfs.CheckedSpecialItem
{
    public enum CheckedSpecialItemStatus : byte
    {
        Successed = 1,
        InvalidHpId,
        InvalidPtId,
        InvalidSinDate,
        InvalidIBirthDay,
        InvalidCheckAge,
        InvalidRaiinNo,
        InvalidOdrInfDetail,
        Failed
    }
}
