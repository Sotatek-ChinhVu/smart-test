using UseCase.Core.Sync.Core;

namespace UseCase.SwapHoken.Save
{
    public class SaveSwapHokenOutputData : IOutputData
    {
        public SaveSwapHokenStatus Status { get; private set; }

        public SaveSwapHokenOutputData(SaveSwapHokenStatus status)
        {
            Status = status;
        }
    }
}
