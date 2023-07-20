using UseCase.Core.Sync.Core;

namespace UseCase.ReceiptCheck
{
    public class ReceiptCheckRecalculationOutputData : IOutputData
    {
        public ReceiptCheckRecalculationOutputData(bool success)
        {
            Success = success;
        }

        public bool Success { get; private set; }
    }
}
