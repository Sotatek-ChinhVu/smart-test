using DevExpress.Models;

namespace DevExpress.Inteface;

public interface IKarte1Export
{
    MemoryStream ExportToPdf(Karte1ExportModel data);
}
