using UseCase.Core.Sync.Core;

namespace UseCase.SmartKartePort.UpdatePort
{
    public sealed class UpdatePortOutputData : IOutputData
    {
        public UpdatePortOutputData(UpdatePortStatus status)
        {
            Status = status;
        }
        public UpdatePortStatus Status { get; private set; }
    }
}
