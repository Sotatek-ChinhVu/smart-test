using DevExpress.DataAccess.ObjectBinding;
using DevExpress.Models;
using DevExpress.Template;
using DevExpress.Xpo.Helpers;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.MedicalExamination.GetHistory;

namespace DevExpress.Export
{
    public class Karte2Export
    {
        public void ExportToPdf()
        {
            var report = new Karte2Template();
            var dataSource = new ObjectDataSource();
            dataSource.DataSource = new Karte2Model()
            {
                Id = "PXP",
                tempObjs = new List<TempObj>() {
                new TempObj(){
                Class = "A1",
                Name = "PXP"
                },
                new TempObj(){
                Class = "A2",
                Name = "PXP2"
                },
                  new TempObj(){
                Class = "A3",
                Name = "PXP3"
                },
                }
            };
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
}
