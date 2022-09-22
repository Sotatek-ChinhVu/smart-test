namespace EmrCloudApi.Tenant.Requests.PatientInfor;

public class SaveListPatientGroupMstRequestItem
{
    public int GroupId { get; set; } = 0;

    public int SortNo { get; set; } = 0;

    public string GroupName { get; set; } = string.Empty;

    public List<SaveListPatientGroupDetailMstRequestItem> Details { get; set; } = new();
}
