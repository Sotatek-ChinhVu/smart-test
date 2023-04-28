using UseCase.Core.Sync.Core;

namespace UseCase.ChartApproval.CheckSaveLogOut
{
    public class CheckSaveLogOutOutputData : IOutputData
    {
        public CheckSaveLogOutOutputData(CheckSaveLogOutStatus status)
        {
            Status = status;
        }

        public CheckSaveLogOutStatus Status { get; private set; }
    }
}
