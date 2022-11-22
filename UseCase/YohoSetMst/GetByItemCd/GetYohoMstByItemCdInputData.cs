using UseCase.Core.Sync.Core;

namespace UseCase.YohoSetMst.GetByItemCd
{
    public class GetYohoMstByItemCdInputData : IInputData<GetYohoMstByItemCdOutputData>
    {
        public int HpId { get; private set; }

        public string ItemCd { get; private set; }

        public int StartDate { get; private set; }

        public GetYohoMstByItemCdInputData(int hpId, string itemCd, int startDate)
        {
            HpId = hpId;
            ItemCd = itemCd;
            StartDate = startDate;
        }
    }
}
