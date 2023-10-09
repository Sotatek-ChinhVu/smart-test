namespace EmrCloudApi.Requests.PatientInfor;

public class UpdateVisitTimesManagementRequest
{
    public long PtId { get; set; }

    public int KohiId { get; set; }

    public int SinYm { get; set; }

    public List<VisitTimesManagementRequestItem> VisitTimesManagementList { get; set; } = new();
}
