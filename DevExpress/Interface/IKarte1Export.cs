using DevExpress.Models;

namespace DevExpress.Interface;

public interface IKarte1Export
{
    Stream ExportToPdf(Karte1ExportModel data);
}
