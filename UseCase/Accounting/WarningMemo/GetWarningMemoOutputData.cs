using Domain.Models.Accounting;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.WarningMemo
{
    public class GetWarningMemoOutputData : IOutputData
    {
        public GetWarningMemoOutputData(List<WarningMemoModel> warningMemoModels, List<RaiinInfModel> raiinInfModels, GetWarningMemoStatus status)
        {
            WarningMemoModels = warningMemoModels;
            RaiinInfModels = raiinInfModels;
            Status = status;
        }

        public List<WarningMemoModel> WarningMemoModels { get; private set; }
        public List<RaiinInfModel> RaiinInfModels { get; private set; }
        public GetWarningMemoStatus Status { get; private set; }
    }
}
