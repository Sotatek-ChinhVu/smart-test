using UseCase.Core.Sync.Core;

namespace UseCase.IsUsingKensa
{
    public class F17CommonInputData : IInputData<F17CommonOutputData>
    {
        public F17CommonInputData(int hpId, string usingKensaItemCd, List<string> usingItemCds, string kensaStdItemCd, string tenItemCd, string itemCd) 
        {
            HpId = hpId;
            UsingKensaItemCd = usingKensaItemCd;
            UsingItemCds = usingItemCds;
            KensaStdItemCd = kensaStdItemCd;
            TenItemCd = tenItemCd;
            ItemCd = itemCd;
        }

        public int HpId { get; set; }

        public string UsingKensaItemCd { get; set; } = string.Empty;

        public List<string> UsingItemCds { get; set; } = new List<string>();

        public string KensaStdItemCd { get; private set; }

        public string TenItemCd { get; private set; }

        public string ItemCd { get; private set; }
    }
}
