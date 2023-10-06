using System.Text.Json.Serialization;

namespace Reporting.ReadRseReportFile.Model;

public class CheckExistTemplateRequestModel
{
    public CheckExistTemplateRequestModel(List<string> templateNameList)
    {
        TemplateNameList = templateNameList;
    }

    [JsonPropertyName("templateNameList")]
    public List<string> TemplateNameList { get; private set; }
}
