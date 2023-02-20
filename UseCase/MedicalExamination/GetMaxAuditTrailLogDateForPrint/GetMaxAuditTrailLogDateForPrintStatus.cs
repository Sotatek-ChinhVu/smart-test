namespace UseCase.MedicalExamination.GetMaxAuditTrailLogDateForPrint
{
    public enum GetMaxAuditTrailLogDateForPrintStatus : byte
    {
        Successed = 1,
        InvalidPtId = 2,
        InvalidRaiinNo = 3,
        InvalidSinDate = 4
    }
}
