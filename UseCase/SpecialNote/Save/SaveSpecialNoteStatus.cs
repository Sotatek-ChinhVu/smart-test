namespace UseCase.SpecialNote.Save
{
    public enum SaveSpecialNoteStatus : byte
    {
        InvalidHpId = 0,
        Successed = 1,
        InvalidPtId = 2,
        InvalidSinDate = 3,
        Failed = 4,
        NoPermissionSaveSummary = 5,
    }
}
