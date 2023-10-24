using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.UpdateYohoSetMst
{
    public sealed class UpdateYohoSetMstOutputData : IOutputData
    {
        public UpdateYohoSetMstOutputData(bool statusUpdate, UpdateYohoSetMstStatus status)
        {
            StatusUpdate = statusUpdate;
            Status = status;
        }
        public bool StatusUpdate { get; private set; }
        public UpdateYohoSetMstStatus Status { get; private set; }
    }
}
