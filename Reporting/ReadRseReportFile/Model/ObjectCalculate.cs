using System.Text.Json.Serialization;

namespace Reporting.ReadRseReportFile.Model;

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
