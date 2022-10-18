namespace Interactor.ExportPDF.Karte2;

public class Karte2Output
{
    public Karte2Output(Karte2Status status)
    {
        Status = status;
        DataStream = new MemoryStream();
    }
    public Karte2Output(Karte2Status status, MemoryStream dataStream)
    {
        Status = status;
        DataStream = dataStream;
    }

    public Karte2Status Status { get; private set; }
    public MemoryStream DataStream { get; private set; }
}
