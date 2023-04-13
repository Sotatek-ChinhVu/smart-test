namespace UseCase.MedicalExamination.GetKensaAuditTrailLog
{
    public enum GetKensaAuditTrailLogStatus : byte
    {
        Successed = 1,
        InvalidRaiinNo,
        InvalidSinDate,
        InvalidPtId,
    }
}
