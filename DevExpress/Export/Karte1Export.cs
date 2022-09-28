using DevExpress.DataAccess.ObjectBinding;
using DevExpress.Models;
using DevExpress.Template;
using DevExpress.XtraPrinting;

namespace DevExpress.Export;

public class Karte1Export
{
    public void ExportToPdf()
    {
        var report = new Karte1Template();
        var dataSource = new ObjectDataSource();
        dataSource.DataSource = new Karte1Model();
       
        report.DataSource = dataSource;
        PdfExportOptions pdfExportOptions = new PdfExportOptions()
        {
            PdfACompatibility = PdfACompatibility.PdfA1b
        };

        // Specify the path for the exported PDF file.  
        string pdfExportFile =
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
            @"\Downloads\" +
            report.Name +
            ".pdf";

        // Export the report.
        report.ExportToPdf(pdfExportFile, pdfExportOptions);
    }
}
