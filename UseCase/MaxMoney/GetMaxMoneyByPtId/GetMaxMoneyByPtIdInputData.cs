using Helper.Common;
using Helper.Extension;
using UseCase.Core.Sync.Core;

namespace UseCase.MaxMoney.GetMaxMoneyByPtId
{
    public class GetMaxMoneyByPtIdInputData : IInputData<GetMaxMoneyByPtIdOutputData>
    {
        public GetMaxMoneyByPtIdInputData(int hpId, long ptId)
        {
            HpId = hpId;
            PtId = ptId;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }
    }
}
