namespace EmrCloudApi.Requests.Family;

public class FamilyRequestItem
{
    public long FamilyId { get; set; }

    public long PtId { get; set; }

    public string ZokugaraCd { get; set; } = string.Empty;

    public long FamilyPtId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string KanaName { get; set; } = string.Empty;

    public int Sex { get; set; }

    public int Birthday { get; set; }

    public int IsDead { get; set; }

    public int IsSeparated { get; set; }

    public string Biko { get; set; } = string.Empty;

    public int SortNo { get; set; }

    public bool IsDeleted { get; set; }

    public List<FamilyRekiRequestItem> ListPtFamilyReki { get; set; } = new();
}
