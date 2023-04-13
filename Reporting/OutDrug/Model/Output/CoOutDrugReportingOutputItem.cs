using System.Text.Json.Serialization;

namespace Reporting.OutDrug.Model.Output;

public class CoOutDrugReportingOutputItem
{
    [JsonPropertyName("singleFieldData")]
    public Dictionary<string, string> SingleFieldData { get; set; } = new();

    [JsonPropertyName("printOutData")]
    public List<PrintOutData> PrintOutData { get; set; } = new();

    [JsonPropertyName("bikoList")]
    public List<string> BikoList { get; set; } = new();

    [JsonPropertyName("printoutType")]
    public int PrintoutType { get; set; }

    [JsonPropertyName("printDataId")]
    public string PrintDataId { get; set; } = string.Empty;

    [JsonPropertyName("printDataPrintType")]
    public int PrintDataPrintType { get; set; }

    [JsonPropertyName("printDataSinDate")]
    public int PrintDataSinDate { get; set; }

    [JsonPropertyName("printDataBunkatuMax")]
    public int PrintDataBunkatuMax { get; set; }

    [JsonPropertyName("printDataSex")]
    public int PrintDataSex { get; set; }

    [JsonPropertyName("printDataHonKeKbn")]
    public int PrintDataHonKeKbn { get; set; }

    [JsonPropertyName("printDataRefillCount")]
    public int PrintDataRefillCount { get; set; }

    [JsonPropertyName("qRData")]
    public string QRData { get; set; } = string.Empty;

    [JsonPropertyName("ptNum")]
    public long PtNum { get; set; }

    [JsonPropertyName("ptName")]
    public string PtName { get; set; } = string.Empty;

    [JsonPropertyName("hpFaxNo")]
    public string HpFaxNo { get; set; } = string.Empty;

    [JsonPropertyName("hpOtherContacts")]
    public string HpOtherContacts { get; set; } = string.Empty;

    [JsonPropertyName("hpTel")]
    public string HpTel { get; set; } = string.Empty;
}
