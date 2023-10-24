using UseCase.Core.Sync.Core;

namespace UseCase.YohoSetMst.GetByItemCd
{
    public class GetYohoMstByItemCdInputData : IInputData<GetYohoMstByItemCdOutputData>
    {
        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public string ItemCd { get; private set; }

        public int StartDate { get; private set; }

        public int SinDate { get; private set; }

        public GetYohoMstByItemCdInputData(int hpId, int userId, string itemCd, int startDate, int sinDate)
        {
            HpId = hpId;
            UserId = userId;
            ItemCd = itemCd;
            StartDate = startDate;
            SinDate = sinDate;
        }
    }
}
