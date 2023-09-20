using UseCase.Core.Sync.Core;

namespace UseCase.IsUsingKensa
{
    public class F17CommonInputData : IInputData<F17CommonOutputData>
    {
        public F17CommonInputData(int hpId, string usingKensaItemCd, List<string> usingItemCds, string kensaStdItemCd) 
        {
            HpId = hpId;
            UsingKensaItemCd = usingKensaItemCd;
            UsingItemCds = usingItemCds;
            KensaStdItemCd = kensaStdItemCd;
        }

        public int HpId { get; set; }

        public string UsingKensaItemCd { get; set; } = string.Empty;

        public List<string> UsingItemCds { get; set; } = new List<string>();

        public string KensaStdItemCd { get; private set; }
    }
}
