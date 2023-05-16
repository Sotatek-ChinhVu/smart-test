using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.CheckIsTenMstUsed
{
    public class CheckIsTenMstUsedOutputData : IOutputData
    {
        public CheckIsTenMstUsedOutputData(CheckIsTenMstUsedStatus status)
        {
            Status = status;
        }

        public CheckIsTenMstUsedStatus Status { get; private set; }
    }
}
