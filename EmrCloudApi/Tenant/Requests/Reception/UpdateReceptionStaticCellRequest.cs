namespace EmrCloudApi.Tenant.Requests.Reception;

public class UpdateReceptionStaticCellRequest
{
    public int SinDate { get; set; }
    public long RaiinNo { get; set; }
    public long PtId { get; set; }
    public string CellName { get; set; } = string.Empty;
    public string CellValue { get; set; } = string.Empty;
}
