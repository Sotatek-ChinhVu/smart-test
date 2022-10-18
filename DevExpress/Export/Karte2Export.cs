using DevExpress.DataAccess.ObjectBinding;
using DevExpress.Models.Karte2;
using DevExpress.Template.Karte2;
using DevExpress.XtraPrinting;

namespace DevExpress.Export;

public class Karte2Export
{
    public MemoryStream ExportToPdf(Karte2ExportModel data)
    {
        try
        {
            var report = new Karte2Template();
            var dataSource = new ObjectDataSource();
            dataSource.DataSource = data;

            report.DataSource = dataSource;
            report.DataMember = "RichTextKarte2Models";

            PdfExportOptions pdfExportOptions = new PdfExportOptions()
            {
                PdfACompatibility = PdfACompatibility.PdfA1b
            };

            // Export the report.
            MemoryStream stream = new();
            report.ExportToPdf(stream, pdfExportOptions);
            return stream;
        }
        catch (Exception)
        {
            return new MemoryStream();
        }
    }
}
