namespace UseCase.HeaderSumaryInfo.Get
{
    public enum GetHeaderSumaryInfoStatus : byte
    {
        InvalidHpId = 0,
        Successed = 1,
        InvalidUserId = 2,
        InvalidPtId = 3,
        InvalidSinDate = 4,
        InvalidRaiinNo = 5,
        NoData = 6,
    }
}
