using System.Drawing;

namespace Reporting.DrugInfo.Model
{
    public class DrugInfoModel
    {
        public bool isDrugGeneration { get; set; }
        public int hpId { get; set; }
        public string hpName { get; set; } = string.Empty;
        public string address1 { get; set; } = string.Empty;
        public string address2 { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string orderDate { get; set; } = string.Empty;
        public long ptNo { get; set; }
        public string ptName { get; set; } = string.Empty;
        public int intAge { get; set; }
        public string age { get; set; } = string.Empty;
        public string sex { get; set; } = string.Empty;
        public string odrYmd { get; set; } = string.Empty;
        public int fsinYmd { get; set; }
        public string usage { get; set; } = string.Empty;
        public string usageSpan { get; set; } = string.Empty;
        public string drgName { get; set; } = string.Empty;
        public string drgComment { get; set; } = string.Empty;
        public string usageComment { get; set; } = string.Empty;
        public int drgKbn { get; set; }
        public int drgType { get; set; }
        public int zaiHouType { get; set; }
        public string amount { get; set; } = string.Empty;
        public string unitName { get; set; } = string.Empty;
        public string usage2 { get; set; } = string.Empty;
        public string picHou { get; set; } = string.Empty;
        public string picZai { get; set; } = string.Empty;
        public string usageSign1 { get; set; } = string.Empty;
        public string usageSign2 { get; set; } = string.Empty;
        public string usageSign3 { get; set; } = string.Empty;
        public string usageSign4 { get; set; } = string.Empty;
        public string usageSign5 { get; set; } = string.Empty;
        public List<DocumentLine> tyui { get; set; } = new();
    }
    public class DocumentLine
    {
        public string text { get; set; } = string.Empty;
        public Color color { get; set; }
    }
}
