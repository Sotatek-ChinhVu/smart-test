using System.ComponentModel.DataAnnotations;

namespace EmrCloudApi.Requests.ExportPDF
{
    public class NameLabelRequest
    {
        [Required]
        public long PtId { get; set; }

        [Required]
        public string KanjiName { get; set; } = string.Empty;

        [Required]
        public int SinDate { get; set; }
    }
}
