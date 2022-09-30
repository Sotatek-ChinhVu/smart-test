using DevExpress.DataAccess.ObjectBinding;
using DevExpress.Interface;
using DevExpress.Mode;
using DevExpress.Models;
using DevExpress.Template;
using DevExpress.XtraPrinting;

namespace DevExpress.Export;

public class Karte1Export : IKarte1Export
{
    public void ExportToPdf(Karte1Model data)
    {
        var report = new Karte1Template();
        var dataSource = new ObjectDataSource();
        
        dataSource.DataSource = data;
        report.DataSource = dataSource;
        report.DataMember = "ListByomeiModels";

        // page 2
        var report_p2 = new Karte1Template_page2();
        var dataSource_p2 = new ObjectDataSource();

        dataSource_p2.DataSource = data;
        report_p2.DataSource = dataSource_p2;
        report_p2.DataMember = "ListByomeiModels";


        PdfExportOptions pdfExportOptions = new PdfExportOptions()
        {
            PdfACompatibility = PdfACompatibility.PdfA1b
        };

        // Specify the path for the exported PDF file.  
        string pdfExportFile =
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
            @"\Downloads\" +
            "demo_file" +
            ".pdf";

        // Export the report.
        report.ExportToPdf(pdfExportFile, pdfExportOptions);
    }
}
