namespace EmrCloudApi.Requests.MstItem;

public class DiseaseSearchRequest
{
    public bool IsPrefix { get; set; } = false;

    public bool IsByomei { get; set; } = false;

    public bool IsSuffix { get; set; } = false;

    public bool IsMisaiyou { get; set; } = false;

    public string Keyword { get; set; } = String.Empty;

    public int Sindate { get; set; }

    public int PageIndex { get; set; } = 1;

    public int PageSize { get; set; } = 1;
}