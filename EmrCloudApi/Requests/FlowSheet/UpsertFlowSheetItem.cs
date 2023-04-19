using System.ComponentModel.DataAnnotations;

namespace EmrCloudApi.Requests.FlowSheet
{
    public class UpsertFlowSheetItem
    {
        [Required]
        public bool Flag { get; set; }

        [Required]
        public long RainNo { get; set; }

        [Required]
        public long PtId { get; set; }

        [Required]
        public int SinDate { get; set; }

        public string Cmt { get; set; } = string.Empty;

        public int TagNo { get; set; }
    }
}