using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.CheckOpenAccounting
{
    public class CheckOpenAccountingOutputData : IOutputData
    {
        public CheckOpenAccountingOutputData(CheckOpenAccountingStatus status)
        {
            Status = status;
        }

        public CheckOpenAccountingStatus Status { get; private set; }
    }
}