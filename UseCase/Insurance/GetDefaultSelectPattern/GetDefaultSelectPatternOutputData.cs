using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.GetDefaultSelectPattern
{
    public class GetDefaultSelectPatternOutputData : IOutputData
    {
        public int HokenPid { get; private set; }

        public GetDefaultSelectPatternStatus Status { get; private set; }

        public GetDefaultSelectPatternOutputData(int hokenPid, GetDefaultSelectPatternStatus status)
        {
            HokenPid = hokenPid;
            Status = status;
        }
    }
}