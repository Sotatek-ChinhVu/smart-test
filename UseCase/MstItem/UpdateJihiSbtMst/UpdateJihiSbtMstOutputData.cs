using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.UpdateJihiSbtMst
{
    public sealed class UpdateJihiSbtMstOutputData : IOutputData
    {
        public UpdateJihiSbtMstOutputData(bool statusUpdate)
        {
            StatusUpdate = statusUpdate;
        }

        public bool StatusUpdate { get; private set; }
    }
}
