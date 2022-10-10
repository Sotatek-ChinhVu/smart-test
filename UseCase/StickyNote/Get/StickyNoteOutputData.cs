using Domain.Models.PtTag;
using UseCase.Core.Sync.Core;

namespace UseCase.StickyNote
{
    public class GetStickyNoteOutputData : IOutputData
    {
        public GetStickyNoteOutputData(List<StickyNoteModel> stickyNoteModels, GetStickyNoteStatus status)
        {
            StickyNoteModels = stickyNoteModels;
            Status = status;
        }

        public GetStickyNoteOutputData(GetStickyNoteStatus status)
        {
            StickyNoteModels = new List<StickyNoteModel>();
            Status = status;
        }

        public List<StickyNoteModel> StickyNoteModels { get; private set; }
        public GetStickyNoteStatus Status { get; private set; }
    }
}
