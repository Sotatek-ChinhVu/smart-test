namespace UseCase.MedicalExamination.ChangeAfterAutoCheckOrder
{
    public enum ChangeAfterAutoCheckOrderStatus : byte
    {
        Successed = 1,
        InvalidHpId,
        InvalidSinDate,
        InvalidUserId,
        InvalidRaiinNo,
        InvalidPtId,
        InvalidOdrInfs
    }
}
