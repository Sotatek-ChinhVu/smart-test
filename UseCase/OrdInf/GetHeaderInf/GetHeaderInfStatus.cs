namespace UseCase.OrdInfs.GetHeaderInf
{
    public enum GetHeaderInfStatus : byte
    {
        InvalidRaiinNo = 0,
        Successed = 1,
        InvalidHpId = 2,
        InvalidPtId = 3,
        InvalidSinDate = 4,
        NoData = 5,
        Failed = 6
    }
}
