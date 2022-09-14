using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.UpdateAdoptedByomei
{
    public class UpdateAdoptedByomeiOutputData : IOutputData
    {
        public UpdateAdoptedByomeiOutputData(bool statusUpdate, UpdateAdoptedByomeiStatus status)
        {
            StatusUpdate = statusUpdate;
            Status = status;
        }

        public bool StatusUpdate { get; private set; }

        public UpdateAdoptedByomeiStatus Status { get; private set; }
    }
}
