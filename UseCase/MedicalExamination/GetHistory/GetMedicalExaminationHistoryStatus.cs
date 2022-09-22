namespace UseCase.MedicalExamination.GetHistory
{
    public enum GetMedicalExaminationHistoryStatus : byte
    {
        InvalidStartPage = 0,
        Successed = 1,
        InvalidHpId = 2,
        InvalidPtId = 3,
        InvalidSinDate = 4,
        InvalidPageSize = 5,
        InvalidDeleteCondition = 6,
        InvalidFilterId = 7,
        InvalidSearchType = 8,
        InvalidSearchCategory = 9,
        InvalidSearchText = 10,
        InvalidUserId = 11,
        NoData = 12,
        Failed = 13
    }
}
