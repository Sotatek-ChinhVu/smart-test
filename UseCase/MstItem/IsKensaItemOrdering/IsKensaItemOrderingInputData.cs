using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.IsKensaItemOrdering
{
    public class IsKensaItemOrderingInputData : IInputData<IsKensaItemOrderingOutputData>
    {
        public IsKensaItemOrderingInputData(int hpId, string tenItemCd)
        {
            HpId = hpId;
            TenItemCd = tenItemCd;
        }

        public int HpId { get; private set; }

        public string TenItemCd { get; private set; }
    }
}
