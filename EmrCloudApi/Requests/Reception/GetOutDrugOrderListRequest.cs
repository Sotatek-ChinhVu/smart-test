namespace EmrCloudApi.Requests.Reception;

public class GetOutDrugOrderListRequest
{
    public bool IsPrintPrescription { get; set; }

    public bool IsPrintAccountingCard { get; set; }

    public int FromDate { get; set; }

    public int ToDate { get; set; }

    public int SinDate { get; set; }
}
