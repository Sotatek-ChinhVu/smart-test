using UseCase.Core.Sync.Core;

namespace UseCase.SwapHoken.Save
{
    public class SaveSwapHokenOutputData : IOutputData
    {
        public SaveSwapHokenStatus Status { get; private set; }

        public string Message { get; private set; }

        public int Type { get; private set; }

        public List<int> SeikyuYms { get; private set; } = new List<int>();

        public SaveSwapHokenOutputData(SaveSwapHokenStatus status, string message, int type, List<int> seikyuYms)
        {
            Status = status;
            Message = message;
            Type = type;
            SeikyuYms = seikyuYms;
        }
    }
}
