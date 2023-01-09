namespace Domain.Models.MstItem;

public class ByomeiMstModel
{
    public ByomeiMstModel(string byomeiCd, string byomeiType, string sbyomei, string kanaName1, int sikanCd, string sikkan, string nanByo, string icd10, string icd102013, bool isAdopted)
    {
        ByomeiCd = byomeiCd;
        ByomeiType = byomeiType;
        Sbyomei = sbyomei;
        KanaName1 = kanaName1;
        SikkanCd = sikanCd;
        Sikkan = sikkan;
        NanByo = nanByo;
        Icd10 = icd10;
        Icd102013 = icd102013;
        IsAdopted = isAdopted;
        KanaName2 = string.Empty;
        KanaName3 = string.Empty;
        KanaName4 = string.Empty;
        KanaName5 = string.Empty;
        KanaName6 = string.Empty;
        KanaName7 = string.Empty;
    }

    public ByomeiMstModel(string byomeiCd, string byomeiType, string sbyomei, string kanaName1, string sikkan, string nanByo, string icd10, string icd102013, bool isAdopted, string kanaName2, string kanaName3, string kanaName4, string kanaName5, string kanaName6, string kanaName7)
    {
        ByomeiCd = byomeiCd;
        ByomeiType = byomeiType;
        Sbyomei = sbyomei;
        KanaName1 = kanaName1;
        SikkanCd = 0;
        Sikkan = sikkan;
        NanByo = nanByo;
        Icd10 = icd10;
        Icd102013 = icd102013;
        IsAdopted = isAdopted;
        KanaName2 = kanaName2;
        KanaName3 = kanaName3;
        KanaName4 = kanaName4;
        KanaName5 = kanaName5;
        KanaName6 = kanaName6;
        KanaName7 = kanaName7;
    }

    public string ByomeiCd { get; private set; }

    public string ByomeiType { get; private set; }

    public string Sbyomei { get; private set; }

    public string KanaName1 { get; private set; }

    public string KanaName2 { get; private set; }

    public string KanaName3 { get; private set; }

    public string KanaName4 { get; private set; }

    public string KanaName5 { get; private set; }

    public string KanaName6 { get; private set; }

    public string KanaName7 { get; private set; }

    public int SikkanCd { get; private set; }

    public string Sikkan { get; private set; }

    public string NanByo { get; private set; }

    public string Icd10 { get; private set; }

    public string Icd102013 { get; private set; }

    public bool IsAdopted { get; private set; }
}
