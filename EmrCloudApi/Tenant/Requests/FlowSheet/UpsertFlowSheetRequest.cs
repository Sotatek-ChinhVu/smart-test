using System.ComponentModel.DataAnnotations;

namespace EmrCloudApi.Tenant.Requests.FlowSheet
{
    public class UpsertFlowSheetRequest
    {
        [Required]
        public List<UpsertFlowSheetItem> Items { get; set; } = new List<UpsertFlowSheetItem>();
    }
}