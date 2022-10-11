using UseCase.Core.Sync.Core;

namespace UseCase.StickyNote
{
    public class SaveStickyNoteOutputData : IOutputData
    {
        public SaveStickyNoteOutputData(bool successed, UpdateStickyNoteStatus status)
        {
            Successed = successed;
            Status = status;
        }

        public bool Successed { get; private set; }
        public UpdateStickyNoteStatus Status { get; private set; }
    }
}
