namespace EmrCloudApi.Requests.Receipt;

public class GetInsuranceReceInfListRequest
{
    public int SeikyuYm { get; set; }

    public int SinYm { get; set; }

    public long PtId { get; set; }

    public int HokenId { get; set; }
}
