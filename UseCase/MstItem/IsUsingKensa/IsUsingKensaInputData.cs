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

        public int HpId { get; set; }

        public string KensaItemCd { get; set; } = string.Empty;

        public List<string> ItemCds { get; set; } = new List<string>();
    }
}
