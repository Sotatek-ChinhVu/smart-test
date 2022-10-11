using UseCase.Core.Sync.Core;

namespace UseCase.StickyNote
{
    public class GetStickyNoteInputData : IInputData<GetStickyNoteOutputData>
    {
        public GetStickyNoteInputData(int hpId, int ptId)
        {
            HpId = hpId;
            PtId = ptId;
        }

        public int HpId { get; private set; }
        public int PtId { get; private set; }
    }
}
