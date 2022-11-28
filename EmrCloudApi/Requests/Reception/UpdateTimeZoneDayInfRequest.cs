namespace EmrCloudApi.Requests.Reception;

public class UpdateTimeZoneDayInfRequest
{
    public int HpId { get; set; }

    public int UserId { get; set; }

    public int SinDate { get; set; }

    public int CurrentTimeKbn { get; set; }

    public int BeforeTimeKbn { get; set; }

    public int UketukeTime { get; set; }
}
