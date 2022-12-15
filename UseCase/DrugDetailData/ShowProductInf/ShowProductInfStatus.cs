namespace UseCase.DrugDetailData.ShowProductInf
{
    public enum ShowProductInfStatus : byte
    {
        Successed = 1,
        InvalidHpId = 2,
        InvalidSinDate = 3,
        InvalidSelectedIndexOfMenuLevel = 4,
        InvalidLevel = 5,
        Failed = 6
    }
}
