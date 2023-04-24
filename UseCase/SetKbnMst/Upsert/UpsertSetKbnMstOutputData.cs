using UseCase.Core.Sync.Core;

namespace UseCase.SetKbnMst.Upsert
{
    public class UpsertSetKbnMstOutputData : IOutputData
    {
        public UpsertSetKbnMstStatus Status { get; private set; }
        public UpsertSetKbnMstOutputData(UpsertSetKbnMstStatus status)
        {
            Status = status;
        }
    }
}