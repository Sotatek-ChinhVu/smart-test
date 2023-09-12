using UseCase.Core.Sync.Core;

namespace UseCase.IsUsingKensa
{
    public class IsUsingKensaInputData : IInputData<IsUsingKensaOutputData>
    {
        public IsUsingKensaInputData(string kensaItemCd, List<string> itemCds) 
        {
            KensaItemCd = kensaItemCd;
            ItemCds = itemCds;
        }

        public string KensaItemCd { get; set; } = string.Empty;

        public List<string> ItemCds { get; set; } = new List<string>();
    }
}
