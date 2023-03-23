namespace EmrCloudApi.Requests.Family;

public class ValidateFamilyListRequest
{
    public long PtId { get; set; }

    public List<FamilyRequestItem> FamilyList { get; set; } = new();
}
