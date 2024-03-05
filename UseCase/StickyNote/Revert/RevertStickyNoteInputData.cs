using UseCase.Core.Sync.Core;

namespace UseCase.StickyNote
{
    public class RevertStickyNoteInputData : IInputData<RevertStickyNoteOutputData>
    {
        public RevertStickyNoteInputData(int hpId, long ptId, int seqNo, int userId)
        {
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
            UserId = userId;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int SeqNo { get; private set; }
        public int UserId { get; private set; }
    }
}
