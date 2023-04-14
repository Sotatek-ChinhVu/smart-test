namespace UseCase.MedicalExamination.GetKensaAuditTrailLog
{
    public enum GetKensaAuditTrailLogStatus : byte
    {
        Successed = 1,
        InvalidHpId,
        InvalidRaiinNo,
        InvalidSinDate,
        InvalidPtId,
    }
}
