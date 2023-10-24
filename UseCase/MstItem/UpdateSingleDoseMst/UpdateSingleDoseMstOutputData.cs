using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.UpdateSingleDoseMst
{
    public sealed class UpdateSingleDoseMstOutputData : IOutputData
    {
        public UpdateSingleDoseMstOutputData(bool statusUpdate)
        {
            StatusUpdate = statusUpdate;
        }

        public bool StatusUpdate { get; private set; }
    }
}
