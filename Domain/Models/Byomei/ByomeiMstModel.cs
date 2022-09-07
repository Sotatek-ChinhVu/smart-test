namespace Domain.Models.Byomei;

public class ByomeiMstModel
{
    public ByomeiMstModel(string byomeiCd, string byomeiCdDisplay, string sbyomei, string kanaName1, string sikkanDisplay, string nanByoDisplay, string icd10Display, string icd102013Display)
    {
        ByomeiCd = byomeiCd;
        ByomeiCdDisplay = byomeiCdDisplay;
        Sbyomei = sbyomei;
        KanaName1 = kanaName1;
        SikkanDisplay = sikkanDisplay;
        NanByoDisplay = nanByoDisplay;
        Icd10Display = icd10Display;
        Icd102013Display = icd102013Display;
    }

    public string ByomeiCd { get; private set; }

    public string ByomeiCdDisplay { get; private set; }

    public string Sbyomei { get; private set; }

    public string KanaName1 { get; private set; }

    public string SikkanDisplay { get; private set; }

    public string NanByoDisplay { get; private set; }

    public string Icd10Display { get; private set; }

    public string Icd102013Display { get; private set; }
}
