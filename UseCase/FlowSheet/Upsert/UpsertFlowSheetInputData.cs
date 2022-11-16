using UseCase.Core.Sync.Core;

namespace UseCase.FlowSheet.Upsert
{
    public class UpsertFlowSheetInputData : IInputData<UpsertFlowSheetOutputData>
    {
        public UpsertFlowSheetInputData(List<UpsertFlowSheetItemInputData> items, int hpId, int userId)
        {
            Items = items;
            HpId = hpId;
            UserId = userId;
        }

        public List<UpsertFlowSheetItemInputData> Items { get; private set; }
        public int HpId { get; private set; }
        public int UserId { get; private set; }

        public List<UpsertFlowSheetItemInputData> ToList()
        {
            return Items;
        }
    }
}
