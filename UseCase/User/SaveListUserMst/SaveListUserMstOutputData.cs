using UseCase.Core.Sync.Core;

namespace UseCase.User.SaveListUserMst
{
    public class SaveListUserMstOutputData : IOutputData
    {
        public SaveListUserMstOutputData(SaveListUserMstStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public SaveListUserMstStatus Status { get; private set; }

        public string Message { get; private set; }
    }
}
