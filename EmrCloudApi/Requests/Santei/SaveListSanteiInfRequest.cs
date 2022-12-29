namespace EmrCloudApi.Requests.Santei;

public class SaveListSanteiInfRequest
{
    public long PtId { get; set; }

    public int SinDate { get; set; }

    public int HokenPId { get; set; }

    public List<SanteiInfDto> ListSanteiInfs { get; set; } = new();
}
