namespace EmrCloudApi.Requests.KensaHistory;

public class GetListKensaInfDetailRequest
{
    public long PtId { get; set; }

    public int SetId { get; set; }

    public int IraiCd { get; set; }

    public int IraiCdStart { get; set; }

    public bool GetGetPrevious { get; set; } = false;

    public bool ShowAbnormalKbn { get; set; } = false;

    public int ItemQuantity { get; set; }

    public List<long> ListSeqNoItems { get; set; } = new();

    public int StartDate { get; set; }

    public int EndDate { get; set; }
}
