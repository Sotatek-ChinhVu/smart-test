namespace EmrCloudApi.Requests.PatientInfor;

public class SaveListPatientGroupMstRequest
{
    public int HpId { get; set; }

    public int UserId { get; set; }

    public List<SaveListPatientGroupMstRequestItem> SaveListPatientGroupMsts { get; set; } = new();
}
