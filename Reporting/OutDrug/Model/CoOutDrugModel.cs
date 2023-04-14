namespace Reporting.OutDrug.Model;

public class CoOutDrugModel
{
    readonly CoOutDrugQRData? _qrData;

    public CoOutDrugModel()
    {
    }

    public CoOutDrugModel(CoOutDrugPrintData printData, CoOutDrugQRData? qrData)
    {
        PrintData = printData;
        _qrData = qrData;
    }

    public CoOutDrugPrintData? PrintData { get; set; }

    public string QRData()
    {
        return _qrData?.QRData ?? string.Empty;
    }
}
