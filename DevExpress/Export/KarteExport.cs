﻿using DevExpress.DataAccess.ObjectBinding;
using DevExpress.Inteface;
using DevExpress.Models.Karte1;
using DevExpress.Models.Karte2;
using DevExpress.Template;
using DevExpress.Template.Karte2;
using DevExpress.XtraPrinting;

namespace DevExpress.Export;

public class KarteExport : IKarteExport
{
    public MemoryStream ExportToPdf(Karte1ExportModel data)
    {
        try
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
        catch (Exception)
        {
            return new MemoryStream();
        }
    }

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
            // Specify the path for the exported PDF file.  
            string pdfExportFile =
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
                @"\Downloads\" +
                "demo_file" +
                ".pdf";

            // Export the report.
            report.ExportToPdf(pdfExportFile, pdfExportOptions);
            MemoryStream stream = new MemoryStream();
            return stream;
        }
        catch (Exception)
        {
            return new MemoryStream();
        }
    }
}
