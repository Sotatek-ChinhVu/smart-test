using UseCase.Core.Sync.Core;

namespace UseCase.Reception.RevertDeleteNoRecept
{
    public class RevertDeleteNoReceptInputData : IInputData<RevertDeleteNoReceptOutputData>
    {
        public RevertDeleteNoReceptInputData(int hpId, long raiinNo, long ptId, int sinDate)
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
