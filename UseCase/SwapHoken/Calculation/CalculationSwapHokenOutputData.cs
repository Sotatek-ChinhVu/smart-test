using UseCase.Core.Sync.Core;

namespace UseCase.SwapHoken.Calculation
{
    public class CalculationSwapHokenOutputData : IOutputData
    {
        public CalculationSwapHokenOutputData(CalculationSwapHokenStatus status, string errText)
        {
            Status = status;
            ErrorText = errText;
        }

        public CalculationSwapHokenStatus Status { get; private set; }

        public string ErrorText { get; private set; }
    }
}
