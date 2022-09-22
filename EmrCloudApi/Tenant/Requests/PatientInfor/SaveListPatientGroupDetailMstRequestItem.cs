namespace EmrCloudApi.Tenant.Requests.PatientInfor;

public class SaveListPatientGroupDetailMstRequestItem
{
    public int GroupId { get; set; } = 0;

    public string GroupCode { get; set; } = string.Empty;

    public long SeqNo { get; set; } = 0;

    public int SortNo { get; set; } = 0;

    public string GroupDetailName { get; set; } = string.Empty;
}
