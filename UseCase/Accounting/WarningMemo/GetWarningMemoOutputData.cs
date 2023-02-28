using Domain.Models.Accounting;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.WarningMemo
{
    public class GetWarningMemoOutputData : IOutputData
    {
        public GetWarningMemoOutputData(List<WarningMemoDto> warningMemoModels, GetWarningMemoStatus status)
        {
            WarningMemoModels = warningMemoModels;
            Status = status;
        }

        public List<WarningMemoDto> WarningMemoModels { get; private set; }
        public GetWarningMemoStatus Status { get; private set; }
    }
}
