using DevExpress.Response.Karte1;

namespace Interactor.ExportPDF.Karte1;

public class Karte1Output
{
    public Karte1Output(Karte1Status status, MemoryStream dataStream)
    {
        Status = status;
        DataStream = dataStream;
    }
    public Karte1Output(Karte1Status status)
    {
        Status = status;
        DataStream = new MemoryStream();
    }

    public Karte1Status Status { get; private set; }
    
    public MemoryStream DataStream { get; private set; }
}
