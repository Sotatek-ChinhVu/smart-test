using UseCase.Core.Sync.Core;

namespace UseCase.PtGroupMst.SaveGroupNameMst
{
    public class SaveGroupNameMstOutputData : IOutputData
    {
        public SaveGroupNameMstOutputData(SaveGroupNameMstStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public string Message { get; private set; }

        public SaveGroupNameMstStatus Status { get; private set; }
    }
}
