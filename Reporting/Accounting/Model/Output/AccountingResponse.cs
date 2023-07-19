using System.Text.Json.Serialization;

namespace Reporting.Accounting.Model.Output;

public class AccountingResponse
{
    public AccountingResponse()
    {
        JobName = string.Empty;
        SystemConfigList = new();
        FileNamePageMap = new();
        AccountingReportingRequestItems = new();
    }
    
    public AccountingResponse(Dictionary<int, string> fileNamePageMap, string jobName, int mode, Dictionary<string, string> systemConfigList, Dictionary<int, List<AccountingOutputModel>> accountingReportingRequestItems)
    {
        FileNamePageMap = fileNamePageMap;
        JobName = jobName;
        Mode = mode;
        SystemConfigList = systemConfigList;
        AccountingReportingRequestItems = accountingReportingRequestItems;
    }

    [JsonPropertyName("fileNamePageMap")]
    public Dictionary<int, string> FileNamePageMap { get; set; }

    [JsonPropertyName("jobName")]
    public string JobName { get; set; }

    [JsonPropertyName("mode")]
    public int Mode { get; set; }

    [JsonPropertyName("systemConfigList")]
    public Dictionary<string, string> SystemConfigList { get; set; }

    [JsonPropertyName("accountingReportingRequestItems")]
    public Dictionary<int, List<AccountingOutputModel>> AccountingReportingRequestItems { get; set; }
}
