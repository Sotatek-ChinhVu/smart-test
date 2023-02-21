namespace EmrCloudApi.Requests.Family;

public class FamilyRekiRequestItem
{
    public long Id { get; set; }

    public string ByomeiCd { get; set; } = string.Empty;

    public string Byomei { get; set; } = string.Empty;

    public string Cmt { get; set; } = string.Empty;

    public int SortNo { get; set; }

    public bool IsDeleted { get; set; }
}
