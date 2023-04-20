using System.Text.Json.Serialization;

namespace Reporting.Accounting.Model.Output;

public class AccountingOutputModel
{
    public AccountingOutputModel(string fileName, int mode, Dictionary<string, string> singleFieldData, List<ListTextModel> listTextModelResult, List<CoSinmeiPrintDataModel> sinmeiPrintDataModelList, Dictionary<string, string> systemConfigList)
    {
        FileName = fileName;
        Mode = mode;
        SingleFieldData = singleFieldData;
        ListTextModelResult = listTextModelResult;
        SinmeiPrintDataModelList = sinmeiPrintDataModelList;
        SystemConfigList = systemConfigList;
    }

    [JsonPropertyName("fileName")]
    public string FileName { get; set; }

    [JsonPropertyName("mode")]
    public int Mode { get; set; }

    [JsonPropertyName("singleFieldList")]
    public Dictionary<string, string> SingleFieldData { get; set; }

    [JsonPropertyName("listTextModelResult")]
    public List<ListTextModel> ListTextModelResult { get; set; }

    [JsonPropertyName("sinmeiPrintDataModelList")]
    public List<CoSinmeiPrintDataModel> SinmeiPrintDataModelList { get; set; }

    [JsonPropertyName("systemConfigList")]
    public Dictionary<string, string> SystemConfigList { get; set; }
}
