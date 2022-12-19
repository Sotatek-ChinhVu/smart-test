namespace UseCase.DrugDetailData.Get
{
    public enum GetDrugDetailDataStatus : byte
    {
        Successed = 1,
        InvalidItemCd = 2,
        InvalidYJCode = 3,
        Failed = 4
    }
}
