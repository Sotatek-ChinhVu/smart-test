using System.Text.Json.Serialization;

namespace Reporting.OutDrug.Model.Output;

public class CoOutDrugReportingOutputData
{
    [JsonPropertyName("data")]
    public List<CoOutDrugReportingOutputItem> Data { get; set; } = new();

    [JsonPropertyName("repeatMax")]
    public int RepeatMax { get; set; }

    [JsonPropertyName("syohosenRefillStrikeLine")]
    public int SyohosenRefillStrikeLine { get; set; }

    [JsonPropertyName("syohosenQRKbn")]
    public int SyohosenQRKbn { get; set; }
}
