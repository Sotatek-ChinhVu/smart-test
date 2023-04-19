namespace Reporting.Accounting.Model;

public class CoSinmeiPrintDataModel
{
    public CoSinmeiPrintDataModel()
    {
        KouiNm = string.Empty;
        MeiData = string.Empty;
        Suuryo = string.Empty;
        Tani = string.Empty;
        Tensu = string.Empty;
        TotalTensu = string.Empty;
        EnTen = string.Empty;
        Kaisu = string.Empty;
        KaisuTani = string.Empty;
        TenKai = string.Empty;
    }

    public string KouiNm { get; set; }
    public string MeiData { get; set; }
    public string Suuryo { get; set; }
    public string Tani { get; set; }
    public string Tensu { get; set; }
    public string TotalTensu { get; set; }
    public string EnTen { get; set; }
    public string Kaisu { get; set; }
    public string KaisuTani { get; set; }
    public string TenKai { get; set; }
}
