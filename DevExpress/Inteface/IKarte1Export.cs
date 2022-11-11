using DevExpress.Models.Karte1;

namespace DevExpress.Inteface;

public interface IKarte1Export
{
    MemoryStream ExportToPdf(Karte1ExportModel data);
}
