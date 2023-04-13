namespace UseCase.MedicalExamination.GetOrdersForOneOrderSheetGroup
{
    public enum GetOrdersForOneOrderSheetGroupStatus : byte
    {
        InvalidOffset = 0,
        Successed = 1,
        InvalidHpId = 2,
        InvalidPtId = 3,
        InvalidSinDate = 4,
        InvalidLimit = 5,
        InvalidOdrKouiKbn = 7,
        InvalidGrpKouiKbn = 8
    }
}
