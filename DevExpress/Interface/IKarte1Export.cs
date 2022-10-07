using DevExpress.Models;

namespace DevExpress.Interface;

public interface IKarte1Export
{
    bool ExportToPdf(Karte1ExportModel data);
}
