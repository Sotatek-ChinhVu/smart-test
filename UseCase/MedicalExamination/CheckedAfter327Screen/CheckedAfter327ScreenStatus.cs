namespace UseCase.MedicalExamination.CheckedAfter327Screen
{
    public enum CheckedAfter327ScreenStatus : byte
    {
        Successed = 1,
        InvalidHpId = 2,
        InvalidPtId = 3,
        InvalidSinDate = 4,
        Failed
    }
}
