namespace EmrCloudApi.Requests.Family;

public class SaveListFamilyRequest
{
    public long PtId { get; set; }

    public List<FamilyRequestItem> ListFamily { get; set; } = new();
}
