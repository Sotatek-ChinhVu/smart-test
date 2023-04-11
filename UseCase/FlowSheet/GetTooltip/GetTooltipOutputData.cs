using UseCase.Core.Sync.Core;

namespace UseCase.FlowSheet.GetTooltip
{
    public class GetTooltipOutputData : IOutputData
    {
        public GetTooltipOutputData(List<(int, string)> values, GetTooltipStatus status)
        {
            Values = values;
            Status = status;
        }

        public List<(int, string)> Values { get; private set; }

        public GetTooltipStatus Status { get; private set; }
    }
}
