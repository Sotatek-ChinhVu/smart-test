namespace UseCase.MedicalExamination.ConvertInputItemToTodayOdr
{
    public enum ConvertInputItemToTodayOrdStatus : byte
    {
        Successed = 1,
        InvalidHpId = 2,
        InvalidDetailInfs = 3,
        InvalidSinDate = 4
    }
}
