using UseCase.Core.Sync.Core;

namespace UseCase.StickyNote
{
    public class DeleteStickyNoteInputData : IInputData<DeleteStickyNoteOutputData>
    {
        public DeleteStickyNoteInputData(int hpId, int ptId, int seqNo)
        {
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
        }

        public int HpId { get; private set; }
        public int PtId { get; private set; }
        public int SeqNo { get; private set; }
    }
}
