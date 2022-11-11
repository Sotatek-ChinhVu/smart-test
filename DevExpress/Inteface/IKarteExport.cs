using DevExpress.Models.Karte1;
using DevExpress.Models.Karte2;

namespace DevExpress.Inteface;

public interface IKarteExport
{
    MemoryStream ExportToPdf(Karte1ExportModel data);

    MemoryStream ExportToPdf(Karte2ExportModel data);
}
