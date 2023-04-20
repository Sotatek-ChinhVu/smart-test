using System.Text.Json.Serialization;

namespace Reporting.Accounting.Model;

public class CoSinmeiPrintDataModel
{
    public CoSinmeiPrintDataModel()
    {
        KouiNm = string.Empty;
        MeiData = string.Empty;
        Suuryo = string.Empty;
        Tani = string.Empty;
        Tensu = string.Empty;
        TotalTensu = string.Empty;
        EnTen = string.Empty;
        Kaisu = string.Empty;
        KaisuTani = string.Empty;
        TenKai = string.Empty;
    }
    [JsonPropertyName("kouiNm")]
    public string KouiNm { get; set; }

    [JsonPropertyName("meiData")]
    public string MeiData { get; set; }

    [JsonPropertyName("suuryo")]
    public string Suuryo { get; set; }

    [JsonPropertyName("tani")]
    public string Tani { get; set; }

    [JsonPropertyName("tensu")]
    public string Tensu { get; set; }

    [JsonPropertyName("totalTensu")]
    public string TotalTensu { get; set; }

    [JsonPropertyName("enTen")]
    public string EnTen { get; set; }

    [JsonPropertyName("kaisu")]
    public string Kaisu { get; set; }

    [JsonPropertyName("kaisuTani")]
    public string KaisuTani { get; set; }

    [JsonPropertyName("tenKai")]
    public string TenKai { get; set; }
}
