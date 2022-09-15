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
        InvalidSearchType = 6,
        InvalidSearchCategory = 7,
        InvalidSearchText = 8,
        NoData = 9,
    }
}
