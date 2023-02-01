using Domain.Models.Accounting;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetWarningMemo
{
    public class GetWarningMemoOutputData : IOutputData
    {
        public GetWarningMemoOutputData(WarningMemoModel warningMemoModel)
        {
            WarningMemoModel = warningMemoModel;
        }

        public WarningMemoModel WarningMemoModel { get; private set; }
    }
}
