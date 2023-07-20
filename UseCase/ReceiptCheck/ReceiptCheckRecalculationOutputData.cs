using UseCase.Core.Sync.Core;

namespace UseCase.ReceiptCheck
{
    public class ReceiptCheckRecalculationOutputData : IOutputData
    {
        public ReceiptCheckRecalculationOutputData(string errorText, RecalculationStatus status)
        {
            ErrorText = errorText;
            Status = status;
        }

        public string ErrorText { get; private set; }
        public RecalculationStatus Status { get; private set; }
    }
}
