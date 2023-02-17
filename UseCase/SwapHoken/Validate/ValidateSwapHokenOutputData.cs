using UseCase.Core.Sync.Core;

namespace UseCase.SwapHoken.Validate
{
    public class ValidateSwapHokenOutputData : IOutputData
    {
        public ValidateSwapHokenStatus Status { get; private set; }

        public string Message { get; private set; }

        public ValidateSwapHokenOutputData(ValidateSwapHokenStatus status, string message)
        {
            Status = status;
            Message = message;
        }
    }
}
