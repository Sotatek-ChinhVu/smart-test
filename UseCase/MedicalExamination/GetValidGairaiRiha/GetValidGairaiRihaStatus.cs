namespace UseCase.MedicalExamination.GetValidGairaiRiha
{
    public enum GetValidGairaiRihaStatus : byte
    {
        Successed = 1,
        InvalidHpId,
        InvalidPtId,
        InvalidRaiinNo,
        InvalidSinDate,
        InvalidSyosaiKbn,
        Failed
    }
}
