namespace EmrCloudApi.Requests.Receipt;

public class RecalculationRequest
{
    public bool IsRecalculationCheckBox { get; set; }

    public bool IsReceiptAggregationCheckBox { get; set; }

    public bool IsCheckErrorCheckBox { get; set; }

    public int SinYm { get; set; }

    public List<long> PtIdList { get; set; } = new();
}
