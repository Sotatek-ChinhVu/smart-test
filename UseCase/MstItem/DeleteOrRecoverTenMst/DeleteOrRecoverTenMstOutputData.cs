using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.DeleteOrRecoverTenMst
{
    public class DeleteOrRecoverTenMstOutputData : IOutputData
    {
        public DeleteOrRecoverTenMstOutputData(DeleteOrRecoverTenMstStatus status)
        {
            Status = status;
        }

        public DeleteOrRecoverTenMstStatus Status { get; private set; }
    }
}
