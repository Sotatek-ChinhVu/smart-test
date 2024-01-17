using EmrCloudApi.Requests.PatientManagement;

namespace EmrCloudApi.Requests.ExportPDF;

public class PatientManagementRequest
{
    public PatientManagementItem PatientManagement { get; set; } = new();
}
