using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.Recaculate
{
    public class RecaculationOutputData : IOutputData
    {
        public RecaculationOutputData(RecaculationStatus status)
        {
            Status = status;
        }

        public RecaculationStatus Status { get; private set; }
    }
}
