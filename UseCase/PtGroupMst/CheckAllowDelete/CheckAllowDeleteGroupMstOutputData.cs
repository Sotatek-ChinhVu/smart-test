using UseCase.Core.Sync.Core;

namespace UseCase.PtGroupMst.CheckAllowDelete
{
    public class CheckAllowDeleteGroupMstOutputData : IOutputData
    {
        public CheckAllowDeleteGroupMstOutputData(CheckAllowDeleteGroupMstStatus status)
        {
            Status = status;
        }

        public CheckAllowDeleteGroupMstStatus Status { get; private set; }
    }
}
