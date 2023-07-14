using UseCase.Core.Sync.Core;

namespace UseCase.Reception.RevertDeleteNoRecept
{
    public class RevertDeleteNoReceptInputData : IInputData<RevertDeleteNoReceptOutputData>
    {
        public RevertDeleteNoReceptInputData(int hpId, long raiinNo)
        {
            HpId = hpId;
            RaiinNo = raiinNo;
        }

        public int HpId { get; private set; }
        public long RaiinNo { get; private set; }
    }
}
