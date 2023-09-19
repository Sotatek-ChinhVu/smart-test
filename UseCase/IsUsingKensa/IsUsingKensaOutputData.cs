using UseCase.Core.Sync.Core;

namespace UseCase.IsUsingKensa
{
    public class IsUsingKensaOutputData : IOutputData
    {
        public IsUsingKensaOutputData(IsUsingKensaStatus status) 
        {
            Status = status;
        }

        public IsUsingKensaStatus Status { get; private set; }
    }
}
