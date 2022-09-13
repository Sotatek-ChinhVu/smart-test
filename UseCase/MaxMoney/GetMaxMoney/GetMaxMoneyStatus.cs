namespace UseCase.MaxMoney.GetMaxMoney
{
    public enum GetMaxMoneyStatus
    {
        Successed = 1,
        DataNotFound = 2,
        InvalidKohiId =3,
        InvalidHpId = 4,
        HokenKohiNotFound = 5,
        HokenKohiNotValidToGet = 6
    }
}
