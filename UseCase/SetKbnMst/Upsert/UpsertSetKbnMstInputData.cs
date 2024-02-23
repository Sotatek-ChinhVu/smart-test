using UseCase.Core.Sync.Core;

namespace UseCase.SetKbnMst.Upsert
{
    public class UpsertSetKbnMstInputData : IInputData<UpsertSetKbnMstOutputData>
    {
        public UpsertSetKbnMstInputData(int hpId, int sinDate, int userId, List<SetKbnMstItem> setKbnMstItems)
        {
            HpId = hpId;
            SinDate = sinDate;
            UserId = userId;
            SetKbnMstItems = setKbnMstItems;
        }

        public int HpId { get; private set; }
        public int SinDate { get; private set; }
        public int UserId { get; private set; }
        public List<SetKbnMstItem> SetKbnMstItems { get; private set; }
    }
}