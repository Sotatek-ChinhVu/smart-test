namespace Reporting.Sijisen.Model
{
    public class CoSijisenPrintDataModel
    {
        public CoSijisenPrintDataModel()
        {
        }

        public bool UnderLine { get; set; }   // 下線
        public string Sikyu { get; set; } = string.Empty;
        public string Data { get; set; } = string.Empty;
        public string Suuryo { get; set; } = string.Empty;
        public string Tani { get; set; } = string.Empty;
    }
}
