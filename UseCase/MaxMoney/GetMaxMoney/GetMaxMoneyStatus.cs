namespace UseCase.MaxMoney.GetMaxMoney
{
    public enum GetMaxMoneyStatus
    {
        Successed = 1,
        InvalidKohiId = 2,
        InvalidHpId = 3,
        InvalidPtId = 4,
        InvalidSinDate = 5,
        HokenKohiNotFound = 6,
        HokenKohiNotValidToGet = 7
    }
}
