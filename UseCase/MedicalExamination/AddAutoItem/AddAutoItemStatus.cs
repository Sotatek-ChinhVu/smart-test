namespace UseCase.MedicalExamination.AddAutoItem
{
    public enum AddAutoItemStatus : byte
    {
        Successed = 1,
        InvalidHpId = 2,
        InvalidUserId = 3,
        InvalidSinDate = 4,
        InvalidAddedAutoItem = 5,
        Failed
    }
}
