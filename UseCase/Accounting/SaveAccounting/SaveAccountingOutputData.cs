using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.SaveAccounting
{
    public class SaveAccountingOutputData : IOutputData
    {
        public SaveAccountingOutputData(SaveAccountingStatus status)
        {
            Status = status;
        }

        public SaveAccountingStatus Status { get; private set; }
    }
}
