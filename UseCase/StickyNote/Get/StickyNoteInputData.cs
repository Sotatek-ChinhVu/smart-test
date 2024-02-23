using UseCase.Core.Sync.Core;

namespace UseCase.StickyNote
{
    public class GetStickyNoteInputData : IInputData<GetStickyNoteOutputData>
    {
        public GetStickyNoteInputData(int hpId, long ptId)
        {
            HpId = hpId;
            PtId = ptId;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
    }
}
