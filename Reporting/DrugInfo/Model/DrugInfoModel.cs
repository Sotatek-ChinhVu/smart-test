using System.Drawing;

namespace Reporting.DrugInfo.Model
{
    public class DrugInfoModel
    {
        public bool IsDrugGeneration { get; set; }
        public int HpId { get; set; }
        public string HpName { get; set; } = string.Empty;
        public string Address1 { get; set; } = string.Empty;
        public string Address2 { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int OrderDate { get; set; }
        public long PtNo { get; set; }
        public string PtName { get; set; } = string.Empty;
        public int IntAge { get; set; }
        public string Age { get; set; } = string.Empty;
        public string Sex { get; set; } = string.Empty;
        public string OdrYmd { get; set; } = string.Empty;
        public int FSinYmd { get; set; }
        public string Usage { get; set; } = string.Empty;
        public string UsageSpan { get; set; } = string.Empty;
        public string DrgName { get; set; } = string.Empty;
        public string DrgComment { get; set; } = string.Empty;
        public string UsageComment { get; set; } = string.Empty;
        public int DrgKbn { get; set; }
        public int DrgType { get; set; }
        public int ZaiHouType { get; set; }
        public string Amount { get; set; } = string.Empty;
        public string UnitName { get; set; } = string.Empty;
        public string Usage2 { get; set; } = string.Empty;
        public string PicHou { get; set; } = string.Empty;
        public string PicZai { get; set; } = string.Empty;
        public string UsageSign1 { get; set; } = string.Empty;
        public string UsageSign2 { get; set; } = string.Empty;
        public string UsageSign3 { get; set; } = string.Empty;
        public string UsageSign4 { get; set; } = string.Empty;
        public string UsageSign5 { get; set; } = string.Empty;
        public List<DocumentLine> Tyui { get; set; } = new();

    }
    public class DocumentLine
    {
        public string Text { get; set; } = string.Empty;
        public Color Color { get; set; }
    }
}
