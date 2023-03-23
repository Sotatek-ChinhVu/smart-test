namespace EmrCloudApi.Responses.Receipt.Dto;

public class ByomeiCheckingDto
{
    public ByomeiCheckingDto(string itemCd, string byomeiCd, string byomei, string fullByomei, int sikkanKbn, int sikkanCd, bool isAdopted, int nanbyoCd)
    {
        ItemCd = itemCd;
        ByomeiCd = byomeiCd;
        Byomei = byomei;
        FullByomei = fullByomei;
        SikkanKbn = sikkanKbn;
        SikkanCd = sikkanCd;
        IsAdopted = isAdopted;
        NanbyoCd = nanbyoCd;
    }

    public string ItemCd { get; private set; }

    public string ByomeiCd { get; private set; }

    public string Byomei { get; private set; }

    public string FullByomei { get; private set; }

    public int SikkanKbn { get; private set; }

    public int SikkanCd { get; private set; }

    public bool IsAdopted { get; private set; }

    public int NanbyoCd { get; private set; }
}
