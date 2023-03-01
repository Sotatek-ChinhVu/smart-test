namespace UseCase.MedicalExamination.SummaryInf
{
    public enum SummaryInfStatus : byte
    {
        Successed = 1,
        InvalidHpId = 2,
        InvalidPtId = 3,
        InvalidSinDate = 4,
        InvalidRaiinNo = 5,
        InvalidUserId = 6,
        InvalidInfoType = 7,
        Failed
    }
}
