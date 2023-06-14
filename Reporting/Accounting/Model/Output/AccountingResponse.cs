﻿using System.Text.Json.Serialization;

namespace Reporting.Accounting.Model.Output;

public class AccountingResponse
{
    public AccountingResponse(string fileName, string jobName, int mode, Dictionary<string, string> systemConfigList, Dictionary<int, List<AccountingOutputModel>> accountingReportingRequestItems)
    {
        FileName = fileName;
        JobName = jobName;
        Mode = mode;
        SystemConfigList = systemConfigList;
        AccountingReportingRequestItems = accountingReportingRequestItems;
    }

    [JsonPropertyName("fileName")]
    public string FileName { get; set; }
    
    [JsonPropertyName("jobName")]
    public string JobName { get; set; }

    [JsonPropertyName("mode")]
    public int Mode { get; set; }

    [JsonPropertyName("systemConfigList")]
    public Dictionary<string, string> SystemConfigList { get; set; }

    [JsonPropertyName("accountingReportingRequestItems")]
    public Dictionary<int, List<AccountingOutputModel>> AccountingReportingRequestItems { get; set; }
}
