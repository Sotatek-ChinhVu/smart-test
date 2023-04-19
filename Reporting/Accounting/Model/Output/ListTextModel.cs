using System.Text.Json.Serialization;

namespace Reporting.Accounting.Model.Output;

public class ListTextModel
{
    public ListTextModel(string listName, short col, short row, string data)
    {
        ListName = listName;
        Col = col;
        Row = row;
        Data = data;
    }

    [JsonPropertyName("listName")]
    public string ListName { get; set; }

    [JsonPropertyName("col")]
    public short Col { get; set; }

    [JsonPropertyName("row")]
    public short Row { get; set; }

    [JsonPropertyName("data")]
    public string Data { get; set; }
}
