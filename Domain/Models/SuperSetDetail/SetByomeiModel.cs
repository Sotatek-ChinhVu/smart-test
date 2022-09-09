namespace Domain.Models.SuperSetDetail;

public class SetByomeiModel
{
    public SetByomeiModel(bool isSyobyoKbn, int sikkanKbn, int nanByoCd, string fullByomei, bool isSuspected, bool isDspRece, bool isDspKarte, string byomeiCmt, string byomeiCd, List<PrefixSuffixModel> prefixSuffixList)
    {
        IsSyobyoKbn = isSyobyoKbn;
        SikkanKbn = sikkanKbn;
        NanByoCd = nanByoCd;
        FullByomei = fullByomei;
        IsSuspected = isSuspected;
        IsDspRece = isDspRece;
        IsDspKarte = isDspKarte;
        ByomeiCmt = byomeiCmt;
        ByomeiCd = byomeiCd;
        PrefixSuffixList = prefixSuffixList;
    }

    public bool IsSyobyoKbn { get; private set; }

    public int SikkanKbn { get; private set; }

    public int NanByoCd { get; private set; }

    public string FullByomei { get; private set; }

    public bool IsSuspected { get; private set; }

    public bool IsDspRece { get; private set; }

    public bool IsDspKarte { get; private set; }

    public string ByomeiCmt { get; private set; }

    public string ByomeiCd { get; private set; }

    public List<PrefixSuffixModel> PrefixSuffixList { get; private set; }
}
