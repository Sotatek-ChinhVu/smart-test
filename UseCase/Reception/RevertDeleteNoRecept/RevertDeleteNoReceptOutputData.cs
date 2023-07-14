using UseCase.Core.Sync.Core;

namespace UseCase.Reception.RevertDeleteNoRecept
{
    public class RevertDeleteNoReceptOutputData : IOutputData
    {
        public RevertDeleteNoReceptOutputData(RevertDeleteNoReceptStatus status)
        {
            Status = status;
        }

        public RevertDeleteNoReceptStatus Status { get; private set; }
    }
}
