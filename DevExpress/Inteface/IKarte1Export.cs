using DevExpress.Models;

namespace DevExpress.Inteface;

public interface IKarte1Export
{
    (MemoryStream, string) ExportToPdf(Karte1ExportModel data);
}
