using DevExpress.DataAccess.ObjectBinding;
using DevExpress.Interface;
using DevExpress.Models;
using DevExpress.Templates;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevExpress.Export
{
    public class Karte2Export : IKarte2Export
    {
        public void ExportToPdf(Karte2ExportModel karte2ExportModel,Stream stream)
        {
            var report = new Karte2ReportTemplate();
            var subReport = new Karte2SubReportTemplate();
            var dataSource = new ObjectDataSource();
            dataSource.DataSource = karte2ExportModel;
            report.DataSource = dataSource;
            subReport.DataSource = dataSource;
            report.DataMember = "HistoryKarteOdrRaiinItems";
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
            var subReport = new Karte2SubReportTemplate();
            var dataSource = new ObjectDataSource();
            dataSource.DataSource = karte2ExportModel;
            report.DataSource = dataSource;
            subReport.DataSource = dataSource;
            report.DataMember = "HistoryKarteOdrRaiinItems";

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
