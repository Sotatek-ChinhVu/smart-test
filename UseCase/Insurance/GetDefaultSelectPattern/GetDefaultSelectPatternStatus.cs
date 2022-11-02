namespace UseCase.Insurance.GetDefaultSelectPattern
{
    public enum GetDefaultSelectPatternStatus : byte
    {
        Failed = 7,
        InvalidSelectedHokenPid = 6,
        InvalidHistoryPid = 5,
        InvalidSinDate = 4,
        InvalidHpId = 3,
        InvalidPtId = 2,
        Successed = 1
    }
}