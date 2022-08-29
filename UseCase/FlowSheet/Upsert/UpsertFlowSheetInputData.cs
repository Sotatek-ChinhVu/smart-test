using UseCase.Core.Sync.Core;

namespace UseCase.FlowSheet.Upsert
{
    public class UpsertFlowSheetInputData : IInputData<UpsertFlowSheetOutputData>
    {
        public UpsertFlowSheetInputData(List<UpsertFlowSheetItemInputData> items)
        {
            Items = items;
        }

        public List<UpsertFlowSheetItemInputData> Items { get; private set; }

        public List<UpsertFlowSheetItemInputData> ToList()
        {
            return Items;
        }
    }
}
