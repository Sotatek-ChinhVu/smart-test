using System.Text.Json.Serialization;

namespace Reporting.Accounting.Model.Output;

public class AccountingOutputModel
{
    public AccountingOutputModel(Dictionary<string, string> singleFieldData, List<ListTextModel> listTextModelResult, List<CoSinmeiPrintDataModel> sinmeiPrintDataModelList)
    {
        SingleFieldData = singleFieldData;
        ListTextModelResult = listTextModelResult;
        SinmeiPrintDataModelList = sinmeiPrintDataModelList;
    }

    [JsonPropertyName("singleFieldList")]
    public Dictionary<string, string> SingleFieldData { get; set; }

    [JsonPropertyName("listTextModelResult")]
    public List<ListTextModel> ListTextModelResult { get; set; }

    [JsonPropertyName("sinmeiPrintDataModelList")]
    public List<CoSinmeiPrintDataModel> SinmeiPrintDataModelList { get; set; }
}
