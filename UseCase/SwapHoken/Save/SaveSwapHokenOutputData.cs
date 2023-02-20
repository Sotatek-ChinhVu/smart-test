using UseCase.Core.Sync.Core;

namespace UseCase.SwapHoken.Save
{
    public class SaveSwapHokenOutputData : IOutputData
    {
        public SaveSwapHokenStatus Status { get; private set; }

        public string Message { get; private set; }

        public int Type { get; private set; }

        public SaveSwapHokenOutputData(SaveSwapHokenStatus status, string message, int type)
        {
            Status = status;
            Message = message;
            Type = type;
        }
    }
}
