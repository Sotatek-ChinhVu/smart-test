namespace EmrCloudApi.Requests.Santei;

public class SaveListSanteiInfRequest
{
    public long PtId { get; set; }

    public int SinDate { get; set; }

    public int HokenPid { get; set; }

    public List<SanteiInfDto> ListSanteiInfs { get; set; } = new();
}
