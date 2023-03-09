using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.Recaculate
{
    public class RecaculationInputData : IInputData<RecaculationOutputData>
    {
        public RecaculationInputData(int hpId, long raiinNo, long ptId, int sinDate)
        {
            HpId = hpId;
            RaiinNo = raiinNo;
            PtId = ptId;
            SinDate = sinDate;
        }

        public int HpId { get; private set; }
        public long RaiinNo { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
    }
}
