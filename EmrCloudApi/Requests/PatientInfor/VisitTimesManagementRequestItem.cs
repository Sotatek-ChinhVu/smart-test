namespace EmrCloudApi.Requests.PatientInfor;

public class VisitTimesManagementRequestItem
{
    public int SinDate { get; set; }

    public int HokenPid { get; set; }

    public int SeqNo { get; set; }

    public bool IsDeleted { get; set; }

    public string SortKey { get; set; } = string.Empty;
}
