using UseCase.Core.Sync.Core;

namespace UseCase.SpecialNote.Get
{
    public class GetSpecialNoteInputData : IInputData<GetSpecialNoteOutputData>
    {
        public GetSpecialNoteInputData(int hpId, long ptId)
        {
            HpId = hpId;
            PtId = ptId;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
    }
}
