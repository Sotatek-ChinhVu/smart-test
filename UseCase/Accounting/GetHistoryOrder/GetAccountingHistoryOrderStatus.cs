namespace UseCase.Accounting.GetHistoryOrder
{
    public enum GetAccountingHistoryOrderStatus
    {
        Successed = 1,
        Failed = 2,
        NoData = 3,
        InvalidStartPage = 4,
        InvalidHpId = 5,
        InvalidPtId = 6,
        InvalidSinDate = 7,
        InvalidPageSize = 8,
        InvalidDeleteCondition = 9,
        InvalidFilterId = 10,
        InvalidSearchType = 11,
        InvalidSearchCategory = 12,
        InvalidSearchText = 13,
        InvalidUserId = 14,
        InvalidRaiinNo = 15,
    }
}
