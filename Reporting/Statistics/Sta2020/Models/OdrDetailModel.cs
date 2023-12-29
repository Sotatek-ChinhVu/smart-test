namespace Reporting.Statistics.Sta2020.Models
{
    public class OdrDetailModel
    {
        public long PtId { get; set; }
        public long RaiinNo { get; set; }
        public int SinYm { get; set; }
        public int SinDate { get; set; }
        public string SinId { get; set; }
        public double Suryo { get; set; }
        public int Count { get; set; }
        public string ItemCd { get; set; }
        public string SrcItemCd { get; set; }
        public string ItemName { get; set; }
        public int KaId { get; set; }
        public string KaSname { get; set; }
        public int TantoId { get; set; }
        public string TantoSname { get; set; }
        public int InoutKbn { get; set; }
    }
}
