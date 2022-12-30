namespace EmrCloudApi.Requests.Santei;

public class SanteiInfDetailDto
{
    public long Id { get; set; }

    public int EndDate { get; set; }

    public int KisanSbt { get; set; }

    public int KisanDate { get; set; }

    public string Byomei { get; set; } = string.Empty;

    public string HosokuComment { get; set; } = string.Empty;

    public string Comment { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }
}
