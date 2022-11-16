using DevExpress.DataAccess.ObjectBinding;
using DevExpress.Inteface;
using DevExpress.Models;
using DevExpress.Template;
using DevExpress.XtraPrinting;

namespace DevExpress.Export;

public class Karte1Export: IKarte1Export
{
    public MemoryStream ExportToPdf(Karte1ExportModel data)
    {
        var report = new Karte1TemplatePage1();
        var dataSource = new ObjectDataSource();
        dataSource.DataSource = data;

        report.DataSource = dataSource;
        report.DataMember = "ListByomeiModelsPage1";

        if (data.ListByomeiModelsPage2.Count > 0)
        {
            report.CreateDocument();
            report.ModifyDocument(report =>
            {
                var page2 = new Karte1TemplatePage2();
                page2.DataSource = dataSource;
                page2.DataMember = "ListByomeiModelsPage2";
                page2.CreateDocument();
                report.AddPages(page2.Pages);
            });
        }

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
