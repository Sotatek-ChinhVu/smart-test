using Domain.Models.Accounting;
using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.WarningMemo
{
    public class GetWarningMemoOutputData : IOutputData
    {
        public GetWarningMemoOutputData(List<WarningMemoModel> warningMemoModels, List<ReceptionDto> receptionDtos, GetWarningMemoStatus status)
        {
            WarningMemoModels = warningMemoModels;
            ReceptionDtos = receptionDtos;
            Status = status;
        }

        public List<WarningMemoModel> WarningMemoModels { get; private set; }
        public List<ReceptionDto> ReceptionDtos { get; private set; }
        public GetWarningMemoStatus Status { get; private set; }
    }
}
