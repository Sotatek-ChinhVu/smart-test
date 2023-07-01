using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.GetDefaultSelectPattern
{
    public class GetDefaultSelectPatternOutputData : IOutputData
    {
        public List<int> HokenPids { get; private set; }

        public GetDefaultSelectPatternStatus Status { get; private set; }

        public GetDefaultSelectPatternOutputData(List<int> hokenPids, GetDefaultSelectPatternStatus status)
        {
            HokenPids = hokenPids;
            Status = status;
        }
    }
}