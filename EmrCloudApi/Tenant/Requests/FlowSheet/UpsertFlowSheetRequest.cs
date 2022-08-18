namespace EmrCloudApi.Tenant.Requests.FlowSheet
{
    public class UpsertFlowSheetRequest
    {
        public List<UpsertFlowSheetItem> Items { get; set; } = new List<UpsertFlowSheetItem>();
    }
}