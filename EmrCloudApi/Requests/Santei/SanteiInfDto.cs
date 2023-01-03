namespace EmrCloudApi.Requests.Santei;

public class SanteiInfDto
{
    public long Id { get; set; }

    public string ItemCd { get; set; } = string.Empty;

    public int AlertDays { get; set; }

    public int AlertTerm { get; set; }

    public bool IsDeleted { get; set; }

    public List<SanteiInfDetailDto> ListSanteInfDetails { get; set; } = new();
}
