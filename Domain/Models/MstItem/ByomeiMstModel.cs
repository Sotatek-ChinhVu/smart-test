namespace Domain.Models.MstItem;

public class ByomeiMstModel
{
    public ByomeiMstModel(string byomeiCd, string byomei, string byomeiType, string sbyomei, string kanaName1, int sikanCd, string sikkan, string nanByo, string icd10, string icd102013, bool isAdopted, int nanbyoCd)
    {
        ByomeiCd = byomeiCd;
        Byomei = byomei;
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
        NanbyoCd = nanbyoCd;
    }
    
    public ByomeiMstModel(string byomeiCd, string byomei, string byomeiType, string sbyomei, string kanaName1, int sikanCd, string sikkan, string nanByo, string icd10, string icd102013, bool isAdopted, int nanbyoCd, string kanaName2)
    {
        ByomeiCd = byomeiCd;
        Byomei = byomei;
        ByomeiType = byomeiType;
        Sbyomei = sbyomei;
        KanaName1 = kanaName1;
        SikkanCd = sikanCd;
        Sikkan = sikkan;
        NanByo = nanByo;
        Icd10 = icd10;
        Icd102013 = icd102013;
        IsAdopted = isAdopted;
        KanaName2 = kanaName2;
        KanaName3 = string.Empty;
        KanaName4 = string.Empty;
        KanaName5 = string.Empty;
        KanaName6 = string.Empty;
        KanaName7 = string.Empty;
        NanbyoCd = nanbyoCd;
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
        Byomei = string.Empty;
    }

    public ByomeiMstModel(string byomei)
    {
        Byomei = byomei;
        ByomeiCd = string.Empty;
        ByomeiType = string.Empty;
        Sbyomei = string.Empty;
        KanaName1 = string.Empty;
        KanaName2 = string.Empty;
        KanaName3 = string.Empty;
        KanaName4 = string.Empty;
        KanaName5 = string.Empty;
        KanaName6 = string.Empty;
        KanaName7 = string.Empty;
        Sikkan = string.Empty;
        NanByo = string.Empty;
        Icd10 = string.Empty;
        Icd102013 = string.Empty;
    }

    public string ByomeiCd { get; private set; }

    public string Byomei { get; private set; }

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

    public int NanbyoCd { get; private set; }

    public int DisplayedOrder {
        get
        {
            if (ByomeiCdDisplay == "接尾語") return 2;
            if (ByomeiCdDisplay == "接頭語") return 1;
            return 0;
        }
    }

    public string ByomeiCdDisplay
    {
        get
        {
            string byomeiCd = ByomeiCd;

            string rs = "";

            if (string.IsNullOrEmpty(byomeiCd)) return rs;

            if (byomeiCd.Length != 4)
            {
                rs = "疾病名";
            }
            else
            {
                if (byomeiCd.StartsWith("8"))
                {
                    rs = "接尾語";
                }
                else if (byomeiCd.StartsWith("9"))
                {
                    rs = "その他";
                }
                else
                {
                    rs = "接頭語";
                }
            }
            return rs;
        }
    }
}
