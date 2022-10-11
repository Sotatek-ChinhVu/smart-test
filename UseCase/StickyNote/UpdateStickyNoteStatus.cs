namespace UseCase.StickyNote
{
    public enum UpdateStickyNoteStatus : byte
    {
        InvalidHpId = 0,
        InvalidPtId = 1,
        InvalidSeqNo = 2,
        InvalidDate = 3,
        InvalidColor = 4,
        InvalidValue = 5,
        Successed = 6,
        Failed = 7,
        InvalidMemo = 8
    }
}
