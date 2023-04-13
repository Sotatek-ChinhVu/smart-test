namespace EmrCloudApi.Requests.ExportPDF;

public class MedicalRecordWebIdRequest
{
    public int HpId { get; set; }

    public long PtId { get; set; }

    public int SinDate { get; set; }
}
