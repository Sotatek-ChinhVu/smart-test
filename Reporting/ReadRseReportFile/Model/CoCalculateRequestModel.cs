using System.Text.Json.Serialization;

namespace Reporting.ReadRseReportFile.Model;

public class CoCalculateRequestModel
{
    public CoCalculateRequestModel(int reportType, string formfile, List<ObjectCalculate> fieldInputList)
    {
        ReportType = reportType;
        Formfile = formfile;
        FieldInputList = fieldInputList;
    }

    [JsonPropertyName("reportType")]
    public int ReportType { get; set; }

    [JsonPropertyName("formfile")]
    public string Formfile { get; set; }

    [JsonPropertyName("fieldInputList")]
    public List<ObjectCalculate> FieldInputList { get; set; }
}
