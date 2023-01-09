namespace UseCase.MedicalExamination.GetAddedAutoItem
{
    public enum GetAddedAutoItemStatus : byte
    {
        Successed = 1,
        InvalidHpId = 2,
        InvalidPtId = 3,
        InvalidSinDate = 4,
        InvalidAddedAutoItem = 5,
        Failed
    }
}
