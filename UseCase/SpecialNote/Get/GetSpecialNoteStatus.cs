namespace UseCase.SpecialNote.Get
{
    public enum GetSpecialNoteStatus : byte
    {
        InvalidHpId = 0,
        Successed = 1,
        InvalidPtId = 2,
        NoData = 3,
    }
}
