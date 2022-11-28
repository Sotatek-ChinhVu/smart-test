namespace EmrCloudApi.Requests.SetMst;

public class SaveSetByomeiRequestItem
{
    public long Id { get; set; } = 0;

    public bool IsSyobyoKbn { get; set; } = false;

    public int SikkanKbn { get; set; } = 0;

    public int NanByoCd { get; set; } = 0;

    public string FullByomei { get; set; } = string.Empty;

    public bool IsSuspected { get; set; } = false;

    public bool IsDspRece { get; set; } = false;

    public bool IsDspKarte { get; set; } = false;

    public string ByomeiCmt { get; set; } = string.Empty;

    public string ByomeiCd { get; set; } = string.Empty;

    public List<PrefixSuffix> PrefixSuffixList { get; set; } = new();
}

public class PrefixSuffix
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}
