using DevExpress.DataAccess.ObjectBinding;
using DevExpress.Interface;
using DevExpress.Mode;
using DevExpress.Models;
using DevExpress.Template;
using DevExpress.XtraPrinting;

namespace DevExpress.Export;

public class Karte1Export : IKarte1Export
{
    public MemoryStream ExportToPdf(Karte1ExportModel data)
    {
        var report = new Karte1Template();
        var dataSource = new ObjectDataSource();

        dataSource.DataSource = data;
        report.DataSource = dataSource;
        report.DataMember = "ListByomeiModels";

        PdfExportOptions pdfExportOptions = new PdfExportOptions()
        {
            PdfACompatibility = PdfACompatibility.PdfA1b
        };

        // Export the report.
        MemoryStream stream = new();
        report.ExportToPdf(stream, pdfExportOptions);
        return stream;
    }
}
