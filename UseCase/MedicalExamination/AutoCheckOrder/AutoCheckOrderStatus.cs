namespace UseCase.MedicalExamination.AutoCheckOrder
{
    public enum AutoCheckOrderStatus : byte
    {
        Successed = 1,
        InvalidHpId,
        InvalidSinDate,
        InvalidPtId,
        InvalidOdrInfs,
    }
}
