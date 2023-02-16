namespace EmrCloudApi.Requests.Family;

public class SaveFamilyListRequest
{
    public long PtId { get; set; }

    public List<FamilyRequestItem> FamilyList { get; set; } = new();
}
