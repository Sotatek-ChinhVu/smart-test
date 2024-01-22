using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.UpdateYosiki
{
    public class UpdateYosikiOutputData : IOutputData
    {
        public UpdateYosikiOutputData(UpdateYosikiStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public UpdateYosikiStatus Status { get; private set; }

        public string Message {  get; private set; }
    }
}
