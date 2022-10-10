using UseCase.Core.Sync.Core;

namespace UseCase.StickyNote
{
    public class RevertStickyNoteOutputData : IOutputData
    {
        public RevertStickyNoteOutputData(bool successed, RevertStickyNoteStatus status)
        {
            Successed = successed;
            Status = status;
        }

        public bool Successed { get; private set; }
        public RevertStickyNoteStatus Status { get; private set; }
    }
}
