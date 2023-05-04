using UseCase.Core.Sync.Core;

namespace UseCase.OrdInfs.CheckOrdInfInDrug
{
    public class CheckOrdInfInDrugInputData : IInputData<CheckOrdInfInDrugOutputData>
    {
        public CheckOrdInfInDrugInputData(long ptId, int hpId, long raiinNo)
        {
            PtId = ptId;
            HpId = hpId;
            RaiinNo = raiinNo;
        }

        public long PtId { get; private set; }
        public int HpId { get; private set; }
        public long RaiinNo { get; private set; }
    }
}
