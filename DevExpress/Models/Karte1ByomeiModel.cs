namespace DevExpress.Mode;

public class Karte1ByomeiModel
{
    public Karte1ByomeiModel(string byomei, string byomeiStartDateWFormat, string byomeiTenkiDateWFormat, string tenkiChusiMaru, string tenkiSiboMaru, string tenkiSonota, string tenkiTiyuMaru)
    {
        Byomei = byomei;
        ByomeiStartDateWFormat = byomeiStartDateWFormat;
        ByomeiTenkiDateWFormat = byomeiTenkiDateWFormat;
        TenkiChusiMaru = tenkiChusiMaru;
        TenkiSiboMaru = tenkiSiboMaru;
        TenkiSonota = tenkiSonota;
        TenkiTiyuMaru = tenkiTiyuMaru;
    }
    public Karte1ByomeiModel()
    {
        Byomei = "byomei";
        ByomeiStartDateWFormat = "byomeiStartDateWFormat";
        ByomeiTenkiDateWFormat = "byomeiTenkiDateWFormat";
        TenkiChusiMaru = "tenkiChusiMaru";
        TenkiSiboMaru = "tenkiSiboMaru";
        TenkiSonota = "tenkiSonota";
        TenkiTiyuMaru = "tenkiTiyuMaru";
    }

    public string Byomei { get; private set; }
    public string ByomeiStartDateWFormat { get; private set; }
    public string ByomeiTenkiDateWFormat { get; private set; }
    public string TenkiChusiMaru { get; private set; }
    public string TenkiSiboMaru { get; private set; }
    public string TenkiSonota { get; private set; }
    public string TenkiTiyuMaru { get; private set; }
}
