namespace EmrCloudApi.Requests.SetMst;

public class GetConversionRequest
{
    public string ItemCd { get; set; } = string.Empty;

    public int SinDate { get; set; }

    public string ItemName { get; set; } = string.Empty;

    public double Quantity { get; set; }

    public string UnitName { get; set; } = string.Empty;
}
