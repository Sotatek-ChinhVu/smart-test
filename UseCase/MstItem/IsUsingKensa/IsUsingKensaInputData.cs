using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.IsUsingKensa
{
    public class IsUsingKensaInputData : IInputData<IsUsingKensaOutputData>
    {
        public IsUsingKensaInputData(int hpId, string kensaItemCd, List<string> itemCds)
        {
            HpId = hpId;
            KensaItemCd = kensaItemCd;
            ItemCds = itemCds;
        }

        public int HpId { get; private set; }

        public string KensaItemCd { get; private set; }

        public List<string> ItemCds { get; private set; }
    }
}
