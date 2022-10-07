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
        try
        {
            var report = new Karte1Template_page1();
            var dataSource = new ObjectDataSource();
            dataSource.DataSource = data;

            report.DataSource = dataSource;
            report.DataMember = "ListByomeiModels_p1";

            if (data.ListByomeiModels_p2.Count > 0)
            {
                report.CreateDocument();
                report.ModifyDocument(report =>
                {
                    var page2 = new Karte1Template_page2();
                    page2.DataSource = dataSource;
                    page2.DataMember = "ListByomeiModels_p2";
                    page2.CreateDocument();
                    report.AddPages(page2.Pages);
                });
            }

            PdfExportOptions pdfExportOptions = new PdfExportOptions()
            {
                PdfACompatibility = PdfACompatibility.PdfA1b
            };

            //// Specify the path for the exported PDF file.  
            //string pdfExportFile =
            //    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
            //    @"\Downloads\" +
            //    data.FileName +
            //    ".pdf";
            //report.ExportToPdf(pdfExportFile, pdfExportOptions);

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
