using Helper.Extension;
using Newtonsoft.Json;
using Reporting.DailyStatic.DB;
using Reporting.DailyStatic.Enum;
using Reporting.DailyStatic.Model;
using Reporting.Mappers.Common;
using Reporting.Statistics.Sta1001.Models;
using Reporting.Statistics.Sta1001.Service;
using Reporting.Statistics.Sta1002.Models;
using Reporting.Statistics.Sta1002.Service;
using Reporting.Statistics.Sta1010.Models;
using Reporting.Statistics.Sta1010.Service;

namespace Reporting.DailyStatic.Service;

public class StatisticService : IStatisticService
{
    private readonly IDailyStatisticCommandFinder _finder;
    private readonly ISta1002CoReportService _sta1002CoReportService;
    private readonly ISta1001CoReportService _sta1001CoReportService;

    public StatisticService(IDailyStatisticCommandFinder finder, ISta1002CoReportService sta1002CoReportService, ISta1010CoReportService sta1010CoReportService, ISta1001CoReportService sta1001CoReportService)
    {
        _finder = finder;
        _sta1002CoReportService = sta1002CoReportService;
        _sta1010CoReportService = sta1010CoReportService;
        _sta1001CoReportService = sta1001CoReportService;
    }

    public CommonReportingRequestModel PrintExecute(int hpId, int menuId, int dateFrom, int dateTo, int timeFrom, int timeTo)
    {
        var configDaily = _finder.GetDailyConfigStatisticMenu(hpId, menuId);

        switch ((StatisticReportType)configDaily.ReportId)
        {
            case StatisticReportType.Sta1001:
                return PrintSta1001(hpId, menuId, configDaily, dateFrom, dateTo, timeFrom, timeTo);
            case StatisticReportType.Sta1002:
                return PrintSta1002(hpId, configDaily, dateFrom, dateTo, timeFrom, timeTo);
            case StatisticReportType.Sta1010:
                return PrintSta1010(hpId, configDaily, dateFrom, dateTo, timeFrom, timeTo);
        }
        return new();
    }

    #region Print Report
    private CommonReportingRequestModel PrintSta1001(int hpId, int menuId, ConfigStatisticModel configDaily, int dateFrom, int dateTo, int timeFrom, int timeTo)
    {
        CoSta1001PrintConf printConf = CreateCoSta1001PrintConf(configDaily, dateFrom, dateTo, timeFrom, timeTo);
        return _sta1001CoReportService.GetSta1001ReportingData(hpId, menuId, dateFrom, dateTo, timeFrom, timeTo);
    }

    private CommonReportingRequestModel PrintSta1002(int hpId, ConfigStatisticModel configDaily, int dateFrom, int dateTo, int timeFrom, int timeTo)
    {
        CoSta1002PrintConf printConf = CreateCoSta1002PrintConf(configDaily, dateFrom, dateTo, timeFrom, timeTo);
        return _sta1002CoReportService.GetSta1002ReportingData(printConf, hpId);
    }

    private CommonReportingRequestModel PrintSta1010(int hpId, ConfigStatisticModel configDaily, int dateFrom, int dateTo, int timeFrom, int timeTo)
    {
        var printConf = CreateCoSta1010PrintConf(configDaily, dateFrom, dateTo, timeFrom, timeTo);
        return _sta1010CoReportService.GetSta1010ReportingData(printConf, hpId);
    }

    #endregion

    #region Create CoStatistic Print
    private CoSta1001PrintConf CreateCoSta1001PrintConf(ConfigStatisticModel configDaily,
                                                                 int dateFrom,
                                                                 int dateTo,
                                                                 int timeFrom,
                                                                 int timeTo)

    {
        CoSta1001PrintConf printConf = new CoSta1001PrintConf(configDaily.MenuId);
        printConf.StartNyukinDate = dateFrom;
        printConf.EndNyukinDate = dateTo;

        if (configDaily.TimeDailyFrom > 0 || configDaily.TimeDailyTo > 0)
        {
            printConf.StartNyukinTime = configDaily.TimeDailyFrom;
            printConf.EndNyukinTime = configDaily.TimeDailyTo;
        }
        else
        {
            printConf.StartNyukinTime = timeFrom;
            printConf.EndNyukinTime = timeTo;
        }

        printConf.FormFileName = configDaily.FormReport;
        printConf.ReportName = configDaily.ReportName;
        printConf.PageBreak1 = configDaily.BreakPage1;
        printConf.PageBreak2 = configDaily.BreakPage2;
        printConf.PageBreak3 = configDaily.BreakPage3;
        printConf.SortOrder1 = configDaily.SortOrder1;
        printConf.SortOrder2 = configDaily.SortOrder2;
        printConf.SortOrder3 = configDaily.SortOrder3;
        printConf.SortOpt1 = configDaily.OrderBy1;
        printConf.SortOpt2 = configDaily.OrderBy2;
        printConf.SortOpt3 = configDaily.OrderBy3;
        printConf.IsTester = configDaily.TestPatient == 1;
        printConf.IsExcludeUnpaid = configDaily.ExcludingUnpaid == 1;
        printConf.UketukeSbtIds = configDaily.UketukeKbnId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.KaIds = configDaily.KaId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.TantoIds = configDaily.UserId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.PaymentMethodCds = configDaily.PaymentKbn.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        return printConf;
    }

    private CoSta1002PrintConf CreateCoSta1002PrintConf(ConfigStatisticModel configDaily,
                                                                 int dateFrom,
                                                                 int dateTo,
                                                                 int timeFrom,
                                                                 int timeTo)

    {
        CoSta1002PrintConf printConf = new CoSta1002PrintConf(configDaily.MenuId);
        printConf.StartNyukinDate = dateFrom;
        printConf.EndNyukinDate = dateTo;

        if (configDaily.TimeDailyFrom > 0 || configDaily.TimeDailyTo > 0)
        {
            printConf.StartNyukinTime = configDaily.TimeDailyFrom;
            printConf.EndNyukinTime = configDaily.TimeDailyTo;
        }
        else
        {
            printConf.StartNyukinTime = timeFrom;
            printConf.EndNyukinTime = timeTo;
        }

        printConf.FormFileName = configDaily.FormReport;
        printConf.ReportName = configDaily.ReportName;
        printConf.PageBreak1 = configDaily.BreakPage1;
        printConf.PageBreak2 = configDaily.BreakPage2;
        printConf.PageBreak3 = configDaily.BreakPage3;
        printConf.IsTester = configDaily.TestPatient == 1;
        printConf.IsExcludeUnpaid = configDaily.ExcludingUnpaid == 1;
        printConf.UketukeSbtIds = configDaily.UketukeKbnId
            .Split(' ')
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => x.AsInteger())
            .ToList();
        printConf.KaIds = configDaily.KaId
            .Split(' ')
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => x.AsInteger())
            .ToList();
        printConf.TantoIds = configDaily.UserId
            .Split(' ')
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => x.AsInteger())
            .ToList();
        printConf.PaymentMethodCds = configDaily.PaymentKbn
            .Split(' ')
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => x.AsInteger())
            .ToList();
        return printConf;
    }

    public static CoSta1010PrintConf CreateCoSta1010PrintConf(ConfigStatisticModel configDaily, int dateFrom, int dateTo, long ptNumFrom, long ptNumTo)
    {
        CoSta1010PrintConf printConf = new CoSta1010PrintConf(configDaily.MenuId);
        printConf.StartSinDate = dateFrom;
        printConf.EndSinDate = dateTo;
        printConf.StartPtNum = ptNumFrom;
        printConf.EndPtNum = ptNumTo;
        printConf.FormFileName = configDaily.FormReport;
        printConf.ReportName = configDaily.ReportName;
        printConf.PageBreak1 = configDaily.BreakPage1;
        printConf.PageBreak2 = configDaily.BreakPage2;
        printConf.SortOrder1 = configDaily.SortOrder1;
        printConf.SortOrder2 = configDaily.SortOrder2;
        printConf.SortOrder3 = configDaily.SortOrder3;
        printConf.SortOpt1 = configDaily.OrderBy1;
        printConf.SortOpt2 = configDaily.OrderBy2;
        printConf.SortOpt3 = configDaily.OrderBy3;
        printConf.IsTester = configDaily.TestPatient == 1;
        printConf.KaIds = configDaily.KaId
            .Split(' ')
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => x.AsInteger())
            .ToList();
        printConf.TantoIds = configDaily.UserId
            .Split(' ')
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => x.AsInteger())
            .ToList();
        printConf.IsNewSeikyu = configDaily.InvoiceKbn == 1;
        printConf.IsDiffSeikyu = printConf.IsNewSeikyu && configDaily.OnlyPatientInvoiceChange == 1;
        printConf.IncludeOutRangeNyukin = configDaily.IncludeOutRangeNyukin == 1;
        printConf.IncludeUnpaid = configDaily.UnPaidVisit == 1;
        printConf.MisyuKbns = configDaily.GetListMisyuKbn();
        return printConf;
    }
    #endregion
}
