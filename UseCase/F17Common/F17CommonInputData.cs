using UseCase.Core.Sync.Core;

namespace UseCase.IsUsingKensa
{
    public class F17CommonInputData : IInputData<F17CommonOutputData>
    {
        public F17CommonInputData(int hpId, string kensaStdItemCd, string itemCd) 
        {
            HpId = hpId;
            KensaStdItemCd = kensaStdItemCd;
            ItemCd = itemCd;
        }

        public int HpId { get; set; }

        public string KensaStdItemCd { get; private set; }

        public string ItemCd { get; private set; }
    }
}
