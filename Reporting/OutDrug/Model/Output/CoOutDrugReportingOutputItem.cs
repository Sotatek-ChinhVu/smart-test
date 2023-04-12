using System.Text.Json.Serialization;

namespace Reporting.OutDrug.Model.Output;

public class CoOutDrugReportingOutputItem
{
    [JsonPropertyName("singleFieldData")]
    public Dictionary<string, string> SingleFieldData { get; set; }

    [JsonPropertyName("printOutData")]
    public List<PrintOutData> PrintOutData { get; set; }

    [JsonPropertyName("bikoList")]
    public List<string> BikoList { get; set; }

    [JsonPropertyName("printoutType")]
    public int PrintoutType { get; set; }

    [JsonPropertyName("printDataId")]
    public string PrintDataId { get; set; }

    [JsonPropertyName("printDataPrintType")]
    public int PrintDataPrintType { get; set; }

    [JsonPropertyName("printDataSinDate")]
    public int PrintDataSinDate { get; set; }
}
