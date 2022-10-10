namespace DevExpress.Models;

public class Karte1ByomeiModel
{
    public Karte1ByomeiModel(string byomei, string byomeiStartDateWFormat, string byomeiTenkiDateWFormat, bool tenkiChusiMaru, bool tenkiSiboMaru, bool tenkiSonota, bool tenkiTiyuMaru)
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
        TenkiChusiMaru = false;
        TenkiSiboMaru = false;
        TenkiSonota = false;
        TenkiTiyuMaru = false;
    }

    public string Byomei { get; private set; }
    public string ByomeiStartDateWFormat { get; private set; }
    public string ByomeiTenkiDateWFormat { get; private set; }
    public bool TenkiChusiMaru { get; private set; }
    public bool TenkiSiboMaru { get; private set; }
    public bool TenkiSonota { get; private set; }
    public bool TenkiTiyuMaru { get; private set; }
}
