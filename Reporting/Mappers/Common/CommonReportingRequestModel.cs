using System.Text.Json.Serialization;

namespace Reporting.Mappers.Common
{
    public class CommonReportingRequestModel
    {
        [JsonPropertyName("formNameList")]
        public List<string> FormNameList { get; set; } = new List<string>();

        [JsonPropertyName("singleFieldList")]
        public Dictionary<string, string> SingleFieldData { get; set; } = new Dictionary<string, string>();

        [JsonPropertyName("visibleFieldList")]
        public Dictionary<string, bool> VisibleFieldData { get; set; } = new Dictionary<string, bool>();

        [JsonPropertyName("wrapFieldList")]
        public Dictionary<string, bool> WrapFieldData { get; set; } = new Dictionary<string, bool>();

        [JsonPropertyName("tableFieldData")]
        public List<Dictionary<string, string>> TableFieldData { get; set; } = new List<Dictionary<string, string>>();
    }
}
