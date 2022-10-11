using Domain.Models.PtTag;
using UseCase.Core.Sync.Core;

namespace UseCase.StickyNote
{
    public class SaveStickyNoteInputData : IInputData<SaveStickyNoteOutputData>
    {
        public SaveStickyNoteInputData(List<StickyNoteModel> stickyNoteModels)
        {
            this.stickyNoteModels = stickyNoteModels;
        }

        public List<StickyNoteModel> stickyNoteModels { get; private set; }
    }
}
