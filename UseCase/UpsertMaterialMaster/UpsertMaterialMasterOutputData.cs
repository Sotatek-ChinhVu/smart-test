using UseCase.Core.Sync.Core;

namespace UseCase.UpsertMaterialMaster
{
    public class UpsertMaterialMasterOutputData : IOutputData
    {
        public UpsertMaterialMasterOutputData(UpsertMaterialMasterStatus status)
        {
            Status = status;
        }

        public UpsertMaterialMasterStatus Status { get; private set; }
    }
}
