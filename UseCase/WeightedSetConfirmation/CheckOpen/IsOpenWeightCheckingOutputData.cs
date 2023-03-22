using UseCase.Core.Sync.Core;

namespace UseCase.WeightedSetConfirmation.CheckOpen
{
    public class IsOpenWeightCheckingOutputData : IOutputData
    {
        public IsOpenWeightCheckingOutputData(IsOpenWeightCheckingStatus status, bool isAllow)
        {
            Status = status;
            IsAllow = isAllow;
        }

        public IsOpenWeightCheckingStatus Status { get; private set; }

        public bool IsAllow { get; private set; }
    }
}
