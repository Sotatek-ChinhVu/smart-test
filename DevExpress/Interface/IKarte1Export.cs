using DevExpress.Models;

namespace DevExpress.Interface;

public interface IKarte1Export
{
    MemoryStream ExportToPdf(Karte1ExportModel data);
}
