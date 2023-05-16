using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.CheckIsTenMstUsed
{
    public class CheckIsTenMstUsedInputData : IInputData<CheckIsTenMstUsedOutputData>
    {
        public CheckIsTenMstUsedInputData(int hpId, string itemCd, int startDate, int endDate)
        {
            HpId = hpId;
            ItemCd = itemCd;
            StartDate = startDate;
            EndDate = endDate;
        }

        public int HpId { get; private set; }

        public string ItemCd { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }
    }
}
