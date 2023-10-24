using EmrCloudApi.Requests.PatientManagement;

namespace EmrCloudApi.Requests.ExportPDF;

public class PatientManagementRequest : ReportRequestBase
{
    public PatientManagementItem PatientManagement { get; set; } = new();
}
