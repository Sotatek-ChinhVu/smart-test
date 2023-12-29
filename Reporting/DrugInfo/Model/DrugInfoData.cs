using System.Text.Json.Serialization;

namespace Reporting.DrugInfo.Model;

public class DrugInfoData
{
    public DrugInfoData(int selectedFormType, int reportType, List<DrugInfoModel> drugInfoList)
    {
        SelectedFormType = selectedFormType;
        ReportType = reportType;
        DrugInfoList = drugInfoList;
    }

    public DrugInfoData()
    {
        DrugInfoList = new();
    }

    [JsonPropertyName("selectedFormType")]
    public int SelectedFormType { get; private set; }

    [JsonPropertyName("reportType")]
    public int ReportType { get; private set; }

    [JsonPropertyName("drugInfoList")]
    public List<DrugInfoModel> DrugInfoList { get; private set; }
}
