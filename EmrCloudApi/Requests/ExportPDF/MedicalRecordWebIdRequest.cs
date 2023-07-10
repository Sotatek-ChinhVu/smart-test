namespace EmrCloudApi.Requests.ExportPDF;

public class MedicalRecordWebIdRequest : ReportRequestBase
{
    public long PtId { get; set; }

    public int SinDate { get; set; }
}
