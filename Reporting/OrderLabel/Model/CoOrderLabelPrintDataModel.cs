namespace Reporting.OrderLabel.Model;

public class CoOrderLabelPrintDataModel
{
    public CoOrderLabelPrintDataModel()
    {
        RpNo = string.Empty;
        InOut = string.Empty;
        Data = string.Empty;
        DataWide = string.Empty;
        Suuryo = string.Empty;
        Tani = string.Empty;
        Comment = string.Empty;
    }

    public string RpNo { get; set; }
    public string InOut { get; set; }
    public string Data { get; set; }
    public string DataWide { get; set; }
    public string Suuryo { get; set; }
    public string Tani { get; set; }
    public string Comment { get; set; }
}
