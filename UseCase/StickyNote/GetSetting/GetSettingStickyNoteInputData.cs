using UseCase.Core.Sync.Core;

namespace UseCase.StickyNote
{
    public class GetSettingStickyNoteInputData : IInputData<GetSettingStickyNoteOutputData>
    {
        public GetSettingStickyNoteInputData(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; private set; }
    }
}
