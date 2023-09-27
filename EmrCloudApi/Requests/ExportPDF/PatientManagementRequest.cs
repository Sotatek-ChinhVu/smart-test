using EmrCloudApi.Requests.PatientManagement;

namespace EmrCloudApi.Requests.ExportPDF;

public class PatientManagementRequest : ReportRequestBase
{
    public int ReportType { get; set; }

    public PatientManagementItem PatientManagement { get; set; } = new();
}
