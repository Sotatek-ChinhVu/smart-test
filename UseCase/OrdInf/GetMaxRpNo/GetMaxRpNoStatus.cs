namespace UseCase.OrdInfs.GetMaxRpNo
{
    public enum GetMaxRpNoStatus : byte
    {
        InvalidRaiinNo = 0,
        Successed = 1,
        InvalidHpId = 2,
        InvalidPtId = 3,
        InvalidSinDate = 4,
        Failed = 5
    }
}
