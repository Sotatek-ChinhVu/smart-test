namespace UseCase.Accounting.CheckAccountingStatus
{
    public enum CheckAccountingStatus
    {
        Successed = 1,
        Failed = 2,
        NoData = 3,
        StateChanged = 4,
        VisitRemoved = 5,
        BillUpdated = 6,
        ValidPaymentAmount = 7,
        ValidThisCredit = 8,
        DateNotVerify = 9,
    }
}
