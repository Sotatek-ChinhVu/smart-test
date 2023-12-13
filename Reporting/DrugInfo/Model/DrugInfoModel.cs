using System.Text.Json.Serialization;

namespace Reporting.DrugInfo.Model
{
    public class DrugInfoModel
    {
        [JsonPropertyName("isDrugGeneration")]
        public bool IsDrugGeneration { get; set; }

        [JsonPropertyName("hpId")]
        public int HpId { get; set; }

        [JsonPropertyName("hpName")]
        public string HpName { get; set; } = string.Empty;

        [JsonPropertyName("address1")]
        public string Address1 { get; set; } = string.Empty;

        [JsonPropertyName("address2")]
        public string Address2 { get; set; } = string.Empty;

        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonPropertyName("orderDate")]
        public string OrderDate { get; set; } = string.Empty;

        [JsonPropertyName("ptNo")]
        public long PtNo { get; set; }

        [JsonPropertyName("ptName")]
        public string PtName { get; set; } = string.Empty;

        [JsonPropertyName("intAge")]
        public int IntAge { get; set; }

        [JsonPropertyName("age")]
        public string Age { get; set; } = string.Empty;

        [JsonPropertyName("sex")]
        public string Sex { get; set; } = string.Empty;

        [JsonPropertyName("odrYmd")]
        public string OdrYmd { get; set; } = string.Empty;

        [JsonPropertyName("fsinYmd")]
        public int FsinYmd { get; set; }

        [JsonPropertyName("usage")]
        public string Usage { get; set; } = string.Empty;

        [JsonPropertyName("usageSpan")]
        public string UsageSpan { get; set; } = string.Empty;

        [JsonPropertyName("drgName")]
        public string DrgName { get; set; } = string.Empty;

        [JsonPropertyName("drgComment")]
        public string DrgComment { get; set; } = string.Empty;

        [JsonPropertyName("usageComment")]
        public string UsageComment { get; set; } = string.Empty;

        [JsonPropertyName("drgKbn")]
        public int DrgKbn { get; set; }

        [JsonPropertyName("drgType")]
        public int DrgType { get; set; }

        [JsonPropertyName("zaiHouType")]
        public int ZaiHouType { get; set; }

        [JsonPropertyName("amount")]
        public string Amount { get; set; } = string.Empty;

        [JsonPropertyName("unitName")]
        public string UnitName { get; set; } = string.Empty;

        [JsonPropertyName("usage2")]
        public string Usage2 { get; set; } = string.Empty;

        [JsonPropertyName("picHou")]
        public string PicHou { get; set; } = string.Empty;

        [JsonPropertyName("picZai")]
        public string PicZai { get; set; } = string.Empty;

        [JsonPropertyName("usageSign1")]
        public string UsageSign1 { get; set; } = string.Empty;

        [JsonPropertyName("usageSign2")]
        public string UsageSign2 { get; set; } = string.Empty;

        [JsonPropertyName("usageSign3")]
        public string UsageSign3 { get; set; } = string.Empty;

        [JsonPropertyName("usageSign4")]
        public string UsageSign4 { get; set; } = string.Empty;

        [JsonPropertyName("usageSign5")]
        public string UsageSign5 { get; set; } = string.Empty;

        [JsonPropertyName("tyui")]
        public List<DocumentLine> Tyui { get; set; } = new();
    }
    public class DocumentLine
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        [JsonPropertyName("color")]
        public string Color { get; set; } = string.Empty;
    }
}
