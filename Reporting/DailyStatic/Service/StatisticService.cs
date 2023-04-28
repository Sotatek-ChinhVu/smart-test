using Helper.Extension;
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
using Reporting.Statistics.Sta2001.Models;
using Reporting.Statistics.Sta2001.Service;
using Reporting.Statistics.Sta2002.Models;
using Reporting.Statistics.Sta2002.Service;

namespace Reporting.DailyStatic.Service;

public class StatisticService : IStatisticService
{
    private readonly IDailyStatisticCommandFinder _finder;
    private readonly ISta1002CoReportService _sta1002CoReportService;
    private readonly ISta1010CoReportService _sta1010CoReportService;
    private readonly ISta2001CoReportService _sta2001CoReportService;
    private readonly ISta1001CoReportService _sta1001CoReportService;
    private readonly ISta2002CoReportService _sta2002CoReportService;

    public StatisticService(IDailyStatisticCommandFinder finder, ISta1002CoReportService sta1002CoReportService, ISta1010CoReportService sta1010CoReportService, ISta2001CoReportService sta2001CoReportService, ISta1001CoReportService sta1001CoReportService, ISta2002CoReportService sta2002CoReportService)
    {
        _finder = finder;
        _sta1002CoReportService = sta1002CoReportService;
        _sta1010CoReportService = sta1010CoReportService;
        _sta2001CoReportService = sta2001CoReportService;
        _sta1001CoReportService = sta1001CoReportService;
        _sta2002CoReportService = sta2002CoReportService;
    }

    public CommonReportingRequestModel PrintExecute(int hpId, int menuId, int monthFrom, int monthTo, int dateFrom, int dateTo, int timeFrom, int timeTo)
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
            case StatisticReportType.Sta2001:
                return PrintSta2001(hpId, configDaily, monthFrom, monthTo);
            case StatisticReportType.Sta2002:
                return PrintSta2002(hpId, configDaily, monthFrom, monthTo);
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

    private CommonReportingRequestModel PrintSta2001(int hpId, ConfigStatisticModel configDaily, int monthFrom, int monthTo)
    {
        var printConf = CreateCoSta2001PrintConf(configDaily, monthFrom, monthTo);
        return _sta2001CoReportService.GetSta2001ReportingData(printConf, hpId);

    }

    private CommonReportingRequestModel PrintSta2002(int hpId, ConfigStatisticModel configDaily, int monthFrom, int monthTo)
    {
        var printConf = CreateCoSta2002PrintConf(configDaily, monthFrom, monthTo);
        return _sta2002CoReportService.GetSta2002ReportingData(printConf, hpId);

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

    private CoSta1010PrintConf CreateCoSta1010PrintConf(ConfigStatisticModel configDaily, int dateFrom, int dateTo, long ptNumFrom, long ptNumTo)
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

    private CoSta2001PrintConf CreateCoSta2001PrintConf(ConfigStatisticModel configDaily,
                                                                int monthFrom,
                                                                int monthTo)

    {
        CoSta2001PrintConf printConf = new CoSta2001PrintConf(configDaily.MenuId);
        printConf.StartNyukinYm = monthFrom;
        printConf.EndNyukinYm = monthTo;
        printConf.FormFileName = configDaily.FormReport;
        printConf.ReportName = configDaily.ReportName;
        printConf.PageBreak1 = configDaily.BreakPage1;
        printConf.PageBreak2 = configDaily.BreakPage2;
        printConf.IsTester = configDaily.TestPatient == 1;
        printConf.IsExcludeUnpaid = configDaily.ExcludingUnpaid == 1;
        printConf.KaIds = configDaily.KaId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.TantoIds = configDaily.UserId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        return printConf;
    }

    private CoSta2002PrintConf CreateCoSta2002PrintConf(ConfigStatisticModel configDaily, int monthFrom, int monthTo)
    {
        CoSta2002PrintConf printConf = new CoSta2002PrintConf(configDaily.MenuId);
        printConf.StartNyukinYm = monthFrom;
        printConf.EndNyukinYm = monthTo;
        printConf.FormFileName = configDaily.FormReport;
        printConf.ReportName = configDaily.ReportName;
        printConf.PageBreak1 = configDaily.BreakPage1;
        printConf.PageBreak2 = configDaily.BreakPage2;
        printConf.IsTester = configDaily.TestPatient == 1;
        printConf.IsExcludeUnpaid = configDaily.ExcludingUnpaid == 1;
        printConf.KaIds = configDaily.KaId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.TantoIds = configDaily.UserId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();

        return printConf;
    }
    #endregion
}
