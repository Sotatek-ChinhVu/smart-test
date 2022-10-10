using UseCase.Core.Sync.Core;

namespace UseCase.StickyNote
{
    public class GetStickyNoteInputData : IInputData<GetStickyNoteOutputData>
    {
        public GetStickyNoteInputData(int hpId, int ptId, int isDeleted)
        {
            HpId = hpId;
            PtId = ptId;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }
        public int PtId { get; private set; }
        public int IsDeleted { get; private set; }
    }
}
