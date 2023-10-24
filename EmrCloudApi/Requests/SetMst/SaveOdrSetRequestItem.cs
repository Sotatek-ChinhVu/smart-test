namespace EmrCloudApi.Requests.SetMst;

public class SaveOdrSetRequestItem
{
    public int SetCd { get; set; }

    public int RowNo { get; set; }

    public string ItemCd { get; set; } = string.Empty;

    public string CmtOpt { get; set; } = string.Empty;

    public double Quantity { get; set; }

    public long SetOrdInfId { get; set; }
}
