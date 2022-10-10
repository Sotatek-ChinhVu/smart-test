using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.FindTenMst
{
    public class FindTenMstInputData : IInputData<FindTenMstOutputData>
    {
        public FindTenMstInputData(int hpId, int sinDate, string itemCd)
        {
            HpId = hpId;
            ItemCd = itemCd;
            SinDate = sinDate;
        }

        public int HpId { get; private set; }
        public int SinDate { get; private set; }
        public string ItemCd { get; private set; }
    }
}
