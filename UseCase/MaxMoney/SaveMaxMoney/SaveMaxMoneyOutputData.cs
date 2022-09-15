using UseCase.Core.Sync.Core;

namespace UseCase.MaxMoney.SaveMaxMoney
{
    public class SaveMaxMoneyOutputData : IOutputData
    {
        public SaveMaxMoneyStatus Status { get; private set; }

        public SaveMaxMoneyOutputData(SaveMaxMoneyStatus status)
        {
            Status = status;
        }
    }
}
