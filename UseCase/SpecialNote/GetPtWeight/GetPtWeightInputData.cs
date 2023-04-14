using UseCase.Core.Sync.Core;

namespace UseCase.SpecialNote.GetPtWeight
{
    public class GetPtWeightInputData : IInputData<GetPtWeightOutputData>
    {
        public GetPtWeightInputData(int sinDate, long ptId)
        {
            SinDate = sinDate;
            PtId = ptId;
        }

        public int SinDate { get; private set; }

        public long PtId { get; private set; }
    }
}
