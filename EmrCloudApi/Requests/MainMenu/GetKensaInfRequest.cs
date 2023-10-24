namespace EmrCloudApi.Requests.MainMenu;

public class GetKensaInfRequest
{
    public int StartDate { get; set; }

    public int EndDate { get; set; }

    public string CenterCd { get; set; } = string.Empty;
}
