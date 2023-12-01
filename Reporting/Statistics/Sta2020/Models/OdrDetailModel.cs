namespace Reporting.Statistics.Sta2020.Models
{
    public class OdrDetailModel
    {
        public long PtId { get; set; }

        public long RaiinNo { get; set; }

        public int SinYm { get; set; }

        public int SinDate { get; set; }

        public string SinId { get; set; } = string.Empty;

        public double Suryo { get; set; }

        public int Count { get; set; }

        public string ItemCd { get; set; } = string.Empty;

        public string SrcItemCd { get; set; } = string.Empty;

        public string ItemName { get; set; } = string.Empty;

        public int KaId { get; set; }

        public string KaSname { get; set; } = string.Empty;

        public int TantoId { get; set; }

        public string TantoSname { get; set; } = string.Empty;

        public int InoutKbn { get; set; }
    }
}
