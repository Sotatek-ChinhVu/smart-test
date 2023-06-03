namespace UseCase.MedicalExamination.GetSinkouCountInMonth
{
    public enum GetSinkouCountInMonthStatus : byte
    {
        Successed = 1,
        InvalidHpId,
        InvalidSinDate,
        InvalidPtId
    }
}
