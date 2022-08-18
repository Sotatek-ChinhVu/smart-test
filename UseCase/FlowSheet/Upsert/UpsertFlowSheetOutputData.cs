using UseCase.Core.Sync.Core;

namespace UseCase.FlowSheet.Upsert
{
    public class UpsertFlowSheetOutputData : IOutputData
    {
        public UpsertFlowSheetOutputData(UpsertFlowSheetStatus status)
        {
            Status = status;
        }

        public UpsertFlowSheetStatus Status { get; private set; }
    }
}
