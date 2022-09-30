namespace UseCase.MstItem.SearchPostCode
{
    public enum SearchPostCodeStatus
    {
        Success = 1,
        Failed = 2,
        NoData = 3,
        InvalidHpId = 4,
        InvalidPostCode = 5,
        InvalidPageIndex = 6,
        InvalidPageSize = 7,
    }
}
