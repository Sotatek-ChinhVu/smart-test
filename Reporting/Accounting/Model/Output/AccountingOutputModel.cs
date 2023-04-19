using System.Text.Json.Serialization;

namespace Reporting.Accounting.Model.Output;

public class AccountingOutputModel
{
    public AccountingOutputModel(Dictionary<string, string> singleFieldData, List<ListTextModel> listTextModelResult, Dictionary<string, string> systemConfigList)
    {
        SingleFieldData = singleFieldData;
        ListTextModelResult = listTextModelResult;
        SystemConfigList = systemConfigList;
    }

    [JsonPropertyName("singleFieldList")]
    public Dictionary<string, string> SingleFieldData { get; set; }

    [JsonPropertyName("listTextModelResult")]
    public List<ListTextModel> ListTextModelResult { get; set; }

    [JsonPropertyName("systemConfigList")]
    public Dictionary<string, string> SystemConfigList { get; set; }
}
