namespace UseCase.MedicalExamination.SearchHistory
{
    public enum SearchHistoryStatus : byte
    {
        Successed = 1,
        InvalidHpId,
        InvalidUserId,
        InvalidPtId,
        InvalidSinDate,
        InvalidCurrentIndex,
        InvalidFilterId,
        InvalidIsDeleted,
        InvalidSearchType,
        Failed
    }
}
