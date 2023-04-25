using System.Text.Json.Serialization;

namespace Reporting.Mappers.Common
{
    public class CommonReportingRequestModel
    {
        [JsonPropertyName("reportType")]
        public int ReportType { get; set; }

        [JsonPropertyName("fileNamePageMap")]
        public Dictionary<string, string> FileNamePageMap { get; set; } = new Dictionary<string, string>();

        [JsonPropertyName("reportConfigModel")]
        public ReportConfigModel ReportConfigModel { get; set; } = new ReportConfigModel();

        [JsonPropertyName("singleFieldList")]
        public Dictionary<string, string> SingleFieldList { get; set; } = new Dictionary<string, string>();

        [JsonPropertyName("tableFieldData")]
        public List<Dictionary<string, CellModel>> TableFieldData { get; set; } = new List<Dictionary<string, CellModel>>();

        [JsonPropertyName("systemConfigList")]
        public Dictionary<string, string> SystemConfigList { get; set; } = new();
    }

    public class CellModel
    {
        public CellModel(string value)
        {
            Value = value;
            IsUnderline = false;
        }

        public CellModel(string value, bool isUnderline)
        {
            Value = value;
            IsUnderline = isUnderline;
        }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("isUnderline")]
        public bool IsUnderline { get; set; }

        [JsonPropertyName("isBold")]
        public bool IsBold { get; set; }

        [JsonPropertyName("isItalic")]
        public bool IsItalic { get; set; }
    }

    public class ReportConfigModel
    {
        [JsonPropertyName("rowCountFieldName")]
        public string RowCountFieldName { get; set; } = string.Empty;

        [JsonPropertyName("visibleFieldList")]
        public Dictionary<string, bool> VisibleFieldList { get; set; } = new Dictionary<string, bool>();

        [JsonPropertyName("wrapFieldList")]
        public Dictionary<string, bool> WrapFieldList { get; set; } = new Dictionary<string, bool>();
    }
}
