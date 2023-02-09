namespace UseCase.MedicalExamination.GetHistoryIndex
{
    public enum GetHistoryIndexStatus : byte
    {
        Successed = 1,
        InvalidHpId,
        InvalidUserId,
        InvalidPtId,
        InvalidFilterId,
        InvalidIsDeleted
    }
}
