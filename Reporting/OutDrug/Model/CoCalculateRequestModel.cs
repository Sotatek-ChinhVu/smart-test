using System.Text.Json.Serialization;

namespace Reporting.OutDrug.Model;

public class CoCalculateRequestModel
{
    public CoCalculateRequestModel(int reportType, List<ObjectCalculate> fieldInputList)
    {
        ReportType = reportType;
        FieldInputList = fieldInputList;
    }

    [JsonPropertyName("reportType")]
    public int ReportType { get; set; }

    [JsonPropertyName("fieldInputList")]
    public List<ObjectCalculate> FieldInputList { get; set; }
}

public class ObjectCalculate
{
    [JsonPropertyName("listName")]
    public string ListName { get; set; }

    [JsonPropertyName("typeEnum")]
    public int TypeEnum { get; set; }

    public ObjectCalculate(string listName, int typeEnum)
    {
        ListName = listName;
        TypeEnum = typeEnum;
    }
}

public enum CalculateTypeEnum : byte
{
    ListFormat = 0,
    ListRowCount = 1,
    GetFormatLength = 2
}
