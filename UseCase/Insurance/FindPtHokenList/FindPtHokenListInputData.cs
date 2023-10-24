using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.FindPtHokenList
{
    public class FindPtHokenListInputData : IInputData<FindPtHokenListOutputData>
    {
        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public FindPtHokenListInputData(int hpId, long ptId, int sinDate)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
        }
    }
}