namespace UseCase.MedicalExamination.GetHistory
{
    public enum GetMedicalExaminationHistoryStatus : byte
    {
        InvalidPageIndex = 0,
        Successed = 1,
        InvalidHpId = 2,
        InvalidPtId = 3,
        InvalidSinDate = 4,
        InvalidPageSize = 5,
        InvalidDeleteCondition = 6,
        InvalidFilterId = 7,
        NoData = 8
    }
}
