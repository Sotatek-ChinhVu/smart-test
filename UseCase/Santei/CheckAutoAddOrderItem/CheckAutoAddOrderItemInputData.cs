using UseCase.Core.Sync.Core;

namespace UseCase.Santei.CheckAutoAddOrderItem
{
    public class CheckAutoAddOrderItemInputData : IInputData<CheckAutoAddOrderItemOutputData>
    {
        public CheckAutoAddOrderItemInputData(int hpId, string itemCd, int sinDate)
        {
            HpId = hpId;
            ItemCd = itemCd;
            SinDate = sinDate;
        }

        public int HpId { get; private set; }
        public string ItemCd { get; private set; }
        public int SinDate { get; private set; }
    }
}
