using UseCase.Core.Sync.Core;

namespace UseCase.FlowSheet.Upsert
{
    public class UpsertFlowSheetInputData : IInputData<UpsertFlowSheetOutputData>
    {
        public UpsertFlowSheetInputData(List<UpsertFlowSheetInputItem> items)
        {
            Items = items;
        }

        public List<UpsertFlowSheetInputItem> Items { get; private set; }

        public List<UpsertFlowSheetInputItem> ToList()
        {
            return Items;
        }
    }
}
