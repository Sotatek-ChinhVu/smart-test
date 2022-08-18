using System.ComponentModel.DataAnnotations;

namespace EmrCloudApi.Tenant.Requests.FlowSheet
{
    public class UpsertFlowSheetItem
    {
        [Required]
        public long RainNo { get; set; }

        [Required]
        public long PtId { get; set; }

        [Required]
        public int SinDate { get; set; }

        [Required]
        public int TagNo { get; set; }

        [Required]
        public int CmtKbn { get; set; }

        public string Text { get; set; } = string.Empty;

        [Required]
        public int TagSeqNo { get; set; }

        [Required]
        public int CmtSeqNo { get; set; }
    }
}