namespace EmrCloudApi.Requests.PatientInfor;

public class UpdateVisitTimesManagementNeedSaveRequest
{
    public long PtId { get; set; }

    public List<UpdateVisitTimesManagementNeedSaveRequestItem> VisitTimesManagementList { get; set; } = new();
}

public class UpdateVisitTimesManagementNeedSaveRequestItem
{
    public int KohiId { get; set; }

    public string SortKey { get; set; } = string.Empty;

    public int SinDate { get; set; }
}