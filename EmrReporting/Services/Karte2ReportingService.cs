﻿using DevExpress.DataAccess.ObjectBinding;
using EmrReporting.DataSources;
using EmrReporting.Reports;

namespace EmrReporting.Services;

public class Karte2ReportingService
{
    public void ExportToPdf()
    {
        var report = new Karte2Report();
        var dataSource = new ObjectDataSource();
        dataSource.DataSource = new Karte2Header
        {
            PtNum = 951,
            KanjiName = "ﾅｶﾆｼ ﾏｻﾋﾛ",
            KanaName = "中西 正廣",
            BirthDay = "昭和 09年01月16日",
            Sex = "男",
            PrintDate = "2022/09/14 15:19",
            PrintStartDate = "2021/01/01(金)",
            PrintEndDate = "---"
        };

        report.DataSource = dataSource;
        //report.WriteRowHeaderInfo("2021/01/09", "受付:08:49 川添Dr 診察:08:49-09:15 川添Dr （承認: 山本Dr 2021 / 02 / 09 18:27）");
        //report.WriteGroupName("初再診");
        //report.WriteActivedOrderCreatedInfo("2021/01/09 08:49 川添Dr", true);
        //report.WriteActivedOrderCreatedInfo("2021/01/09 08:49 川添Dr");
        report.TryWriteActivedOrderCreatedInfo("＊2021/01/09 08:49 川添Dr", true);
        report.TryWriteActivedOrderCreatedInfo("＊2021/01/09 08:49 川添Dr");
        report.TryWriteActivedOrderCreatedInfo("＊2021/01/09 08:49 川添Dr", true);
        //report.WriteText("＊");
        report.CreateDocument();
        for (int i = 0; i < 1; i++)
        {
            report.ModifyDocument(modifier =>
            {
                var report = new Karte2Report();
                report.DataSource = dataSource;
                report.CreateDocument();
                modifier.AddPages(report.Pages);
            });
        }
        //report.LeftCell.Text = "1\n2\n1\n2\n1\n2\n1\n2\n1\n2\n1\n2\n1\n2\n1\n2\n1\n2\n1\n2\n1\n2\n1\n2\n1\n2\n1\n2\n1\n2\n1\n2\n1\n2\n1\n2\n1\n2\n1\n2\n";
        report.ExportToPdf("EmployeeReport.pdf");
    }

    //public void ExportToPdf()
    //{
    //    var report = new EmployeeReport();
    //    var dataSource = new ObjectDataSource();
    //    dataSource.DataSource = new EmployeeList();
    //    dataSource.DataMember = "Items";

    //    report.DataSource = dataSource;
    //    report.ExportToPdf("EmployeeReport.pdf");
    //}
}
