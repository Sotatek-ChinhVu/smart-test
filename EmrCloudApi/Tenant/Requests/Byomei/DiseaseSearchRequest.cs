namespace EmrCloudApi.Tenant.Requests.Byomei;

public class DiseaseSearchRequest
{
    public bool IsPrefix { get; set; } = false;

    public bool IsByomei { get; set; } = false;

    public bool IsSuffix { get; set; } = false;

    public string Keyword { get; set; } = String.Empty;

    public int PageIndex { get; set; } = 1;

    public int PageCount { get; set; } = 1;
}