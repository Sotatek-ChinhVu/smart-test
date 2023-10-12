using UseCase.Core.Sync.Core;

namespace UseCase.Logger
{
    public class WriteLogOutputData : IOutputData
    {
        public WriteLogOutputData(WriteLogStatus status)
        {
            Status = status;
        }

        public WriteLogStatus Status { get; private set; }
    }
}
