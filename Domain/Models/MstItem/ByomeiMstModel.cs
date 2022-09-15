namespace Domain.Models.MstItem;

public class ByomeiMstModel
{
    public ByomeiMstModel(string byomeiCd, string byomeiType, string sbyomei, string kanaName1, string sikkan, string nanByo, string icd10, string icd102013)
    {
        ByomeiCd = byomeiCd;
        ByomeiType = byomeiType;
        Sbyomei = sbyomei;
        KanaName1 = kanaName1;
        Sikkan = sikkan;
        NanByo = nanByo;
        Icd10 = icd10;
        Icd102013 = icd102013;
    }

    public string ByomeiCd { get; private set; }

    public string ByomeiType { get; private set; }

    public string Sbyomei { get; private set; }

    public string KanaName1 { get; private set; }

    public string Sikkan { get; private set; }

    public string NanByo { get; private set; }

    public string Icd10 { get; private set; }

    public string Icd102013 { get; private set; }
}
