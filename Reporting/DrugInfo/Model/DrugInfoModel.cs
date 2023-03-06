using System.Drawing;

namespace Reporting.DrugInfo.Model
{
    public class DrugInfoModel
    {
        public bool IsDrugGeneration { get; set; }
        public int HpId { get; set; }
        public string HpName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Phone { get; set; }
        public int OrderDate { get; set; }
        public long PtNo { get; set; }
        public string PtName { get; set; }
        public int IntAge { get; set; }
        public string Age { get; set; }
        public string Sex { get; set; }
        public string OdrYmd { get; set; }
        public int FSinYmd { get; set; }
        public string Usage { get; set; }
        public string UsageSpan { get; set; }
        public string DrgName { get; set; }
        public string DrgComment { get; set; }
        public string UsageComment { get; set; }
        public int DrgKbn { get; set; }
        public int DrgType { get; set; }
        public int ZaiHouType { get; set; }
        public string Amount { get; set; }
        public string UnitName { get; set; }
        public string Usage2 { get; set; }
        public string PicHou { get; set; }
        public string PicZai { get; set; }
        public string UsageSign1 { get; set; }
        public string UsageSign2 { get; set; }
        public string UsageSign3 { get; set; }
        public string UsageSign4 { get; set; }
        public string UsageSign5 { get; set; }
        public List<DocumentLine> Tyui { get; set; }

    }
    public class DocumentLine
    {
        public string Text { get; set; }
        public Color Color { get; set; }
    }
}
