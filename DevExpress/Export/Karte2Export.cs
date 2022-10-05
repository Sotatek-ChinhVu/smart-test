using DevExpress.DataAccess.ObjectBinding;
using DevExpress.Interface;
using DevExpress.Models;
using DevExpress.Templates;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports;
using DevExpress.XtraReports.UI;

namespace DevExpress.Export
{
    public class Karte2Export : IKarte2Export
    {
        public void ExportToPdf(Karte2ExportModel karte2ExportModel,Stream stream)
        {
            var report = new Karte2ReportTemplate();
            var dataSource = new ObjectDataSource();
            dataSource.DataSource = karte2ExportModel;
            report.DataSource = dataSource;
            PdfExportOptions pdfExportOptions = new PdfExportOptions()
            {
                PdfACompatibility = PdfACompatibility.PdfA1b
            };

            // Export the report.
            report.ExportToPdf(stream, pdfExportOptions);
        }

        public void ExportToPdf(Karte2ExportModel karte2ExportModel)
        {
            var report = new Karte2ReportTemplate();

            var dataSource = new ObjectDataSource();
            var dataSourceRichText = new ObjectDataSource();
            var dataSourceHoken= new ObjectDataSource();

            dataSource.DataSource = karte2ExportModel;
            dataSourceRichText.DataSource = karte2ExportModel.RichTextKarte2Models;
            dataSourceHoken.DataSource = karte2ExportModel.RichTextKarte2Models.SelectMany(x => x.GroupNameKarte2Models).ToList();


            report.DataSource = dataSource;
            XRSubreport subReportRichtext = report.xrSubreport2;
            subReportRichtext.ReportSource.DataSource = dataSourceRichText;
            XRSubreport subReportHoken = subReportRichtext.ReportSource.Bands[BandKind.Detail].FindControl("xrSubreport1", true) as XRSubreport;
            subReportHoken.ReportSource.DataSource = dataSourceHoken;

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
}
