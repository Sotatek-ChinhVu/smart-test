using UseCase.Core.Sync.Core;

namespace UseCase.SetKbnMst.Upsert
{
    public class UpsertSetKbnMstInputData : IInputData<UpsertSetKbnMstOutputData>
    {
        public UpsertSetKbnMstInputData(int sinDate, int userId, List<SetKbnMstItem> setKbnMstItems)
        {
            SinDate = sinDate;
            UserId = userId;
            SetKbnMstItems = setKbnMstItems;
        }

        public int SinDate { get; private set; }
        public int UserId { get; private set; }
        public List<SetKbnMstItem> SetKbnMstItems { get; private set; }
    }
}