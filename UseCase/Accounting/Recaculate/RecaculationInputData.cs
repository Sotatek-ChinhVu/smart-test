using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.Recaculate
{
    public class RecaculationInputData : IInputData<RecaculationOutputData>
    {
        public RecaculationInputData(int hpId, long ptId, int sinDate)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
    }
}
