using Reporting.OutDrug.Model;

namespace EmrCloudApi.Requests.ExportPDF;

public class OutDrugRequest
{
    public int HpId { get; set; }
    public string JsonOutDrug { get; set; } = string.Empty;
}
