namespace UseCase.StickyNote
{
    public enum RevertStickyNoteStatus : byte
    {
        InvalidHpId = 0,
        InvalidPtId = 1,
        InvalidSeqNo = 2,
        Successed = 3,
        Failed = 4,
    }
}
