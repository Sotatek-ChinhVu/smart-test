using Domain.Models.PtTag;
using UseCase.Core.Sync.Core;

namespace UseCase.StickyNote
{
    public class SaveStickyNoteInputData : IInputData<SaveStickyNoteOutputData>
    {
        public SaveStickyNoteInputData(List<StickyNoteModel> stickyNoteModels, int userId)
        {
            this.stickyNoteModels = stickyNoteModels;
            UserId = userId;
        }

        public List<StickyNoteModel> stickyNoteModels { get; private set; }

        public int UserId { get; private set; }
    }
}
