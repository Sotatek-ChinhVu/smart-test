using UseCase.Core.Sync.Core;

namespace UseCase.StickyNote
{
    public class GetSettingStickyNoteInputData : IInputData<GetSettingStickyNoteOutputData>
    {
        public GetSettingStickyNoteInputData(int hpId, int userId)
        {
            HpId = hpId;
            UserId = userId;
        }

        public int HpId { get; private set; }
        public int UserId { get; private set; }
    }
}
