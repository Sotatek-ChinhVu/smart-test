namespace UseCase.Set.GetList
{
    public enum GetSetListStatus : byte
    {
        Successed = 1,
        NoData = 2,
        InvalidHpId = 3,
        InvalidSetKbn = 4,
        InvalidSetKbnEdaNo = 5,
        InvalidSinDate = 6
    }
}