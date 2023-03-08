using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetAccountingSystemConf
{
    public class GetAccountingConfigInputData : IInputData<GetAccountingConfigOutputData>
    {
        public GetAccountingConfigInputData(int hpId, long ptId, long raiinNo, int sumAdjust)
        {
            HpId = hpId;
            PtId = ptId;
            RaiinNo = raiinNo;
            SumAdjust = sumAdjust;
        }
        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public long RaiinNo { get; private set; }
        public int SumAdjust { get; private set; }
    }
}
