using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.HokenPatternUsed
{
    public class HokenPatternUsedOutputData : IOutputData
    {
        public bool IsUsed { get; private set; }

        public HokenPatternUsedStatus Status { get; private set; }

        public HokenPatternUsedOutputData(bool isUsed, HokenPatternUsedStatus status)
        {
            IsUsed = isUsed;
            Status = status;
        }
    }
}
