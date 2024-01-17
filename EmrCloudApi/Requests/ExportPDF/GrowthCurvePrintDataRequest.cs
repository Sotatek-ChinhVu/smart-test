namespace EmrCloudApi.Requests.ExportPDF
{
    public class GrowthCurvePrintDataRequest
    {
        public int Type { get; set; }
        public long PtNum { get; set; }
        public long PtId { get; set; }
        public string PtName { get; set; } = string.Empty;
        public int Sex { get; set; }
        public int BirthDay { get; set; }
        public int PrintMode { get; set; }
        public int PrintDate { get; set; }
        public bool WeightVisible { get; set; } = true;
        public bool HeightVisible { get; set; } = true;
        public bool Per50 { get; set; } = true;
        public bool Per25 { get; set; } = true;
        public bool Per10 { get; set; } = true;
        public bool Per3 { get; set; } = true;

        public bool SDAvg { get; set; } = true;
        public bool SD1 { get; set; } = true;
        public bool SD2 { get; set; } = true;
        public bool SD25 { get; set; } = true;
        public bool Legend { get; set; } = true;

        public int Scope { get; set; }
    }
}
