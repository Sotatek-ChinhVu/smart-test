using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.DeleteOrRecoverTenMst
{
    public class DeleteOrRecoverTenMstOutputData : IOutputData
    {
        public DeleteOrRecoverTenMstOutputData(DeleteOrRecoverTenMstStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public DeleteOrRecoverTenMstStatus Status { get; private set; }

        public string Message { get; private set; }
    }
}
