using System.Text.Json.Serialization;

namespace Reporting.OutDrug.Model.Output;

public class CoOutDrugReportingOutputData
{
    [JsonPropertyName("data")]
    public List<CoOutDrugReportingOutputItem> Data { get; set; }

    [JsonPropertyName("repeatMax")]
    public int RepeatMax { get; set; }

    [JsonPropertyName("syohosenRefillStrikeLine")]
    public int SyohosenRefillStrikeLine { get; set; }
}
