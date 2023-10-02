using Helper.Extension;
using Reporting.CommonMasters.Enums;
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
using Reporting.Statistics.Sta2003.Models;
using Reporting.Statistics.Sta2003.Service;
using Reporting.Statistics.Sta2010.Models;
using Reporting.Statistics.Sta2010.Service;
using Reporting.Statistics.Sta2011.Models;
using Reporting.Statistics.Sta2011.Service;
using Reporting.Statistics.Sta2021.Models;
using Reporting.Statistics.Sta2021.Service;
using Reporting.Statistics.Sta3020.Models;
using Reporting.Statistics.Sta3020.Service;
using Reporting.Statistics.Sta3071.Models;
using Reporting.Statistics.Sta3071.Service;
using Reporting.Statistics.Sta3080.Models;
using Reporting.Statistics.Sta3080.Service;
using Reporting.Statistics.Sta2020.Models;
using Reporting.Statistics.Sta2020.Service;
using Reporting.Statistics.Sta3010.Service;
using Reporting.Statistics.Sta3010.Models;
using Reporting.Statistics.Sta3030.Service;
using Reporting.Statistics.Sta3030.Models;
using Reporting.Statistics.Sta3001.Models;
using Reporting.Statistics.Sta3001.Service;
using Reporting.Statistics.Sta3040.Service;
using Reporting.Statistics.Sta3040.Models;
using Reporting.Statistics.Sta3041.Models;
using Reporting.Statistics.Sta3041.Service;
using Reporting.Statistics.Sta3050.Service;
using Reporting.Statistics.Sta3050.Models;
using Newtonsoft.Json;
using Reporting.Statistics.Sta3060.Models;
using Reporting.Statistics.Sta3060.Service;
using Reporting.Statistics.Sta3061.Models;
using Reporting.Statistics.Sta3061.Service;
using Reporting.Statistics.Sta3070.Models;
using Reporting.Statistics.Sta3070.Service;

namespace Reporting.DailyStatic.Service;

public class StatisticService : IStatisticService
{
    private readonly IDailyStatisticCommandFinder _finder;
    private readonly ISta1002CoReportService _sta1002CoReportService;
    private readonly ISta1010CoReportService _sta1010CoReportService;
    private readonly ISta2001CoReportService _sta2001CoReportService;
    private readonly ISta2003CoReportService _sta2003CoReportService;
    private readonly ISta1001CoReportService _sta1001CoReportService;
    private readonly ISta2002CoReportService _sta2002CoReportService;
    private readonly ISta2010CoReportService _sta2010CoReportService;
    private readonly ISta2011CoReportService _sta2011CoReportService;
    private readonly ISta2021CoReportService _sta2021CoReportService;
    private readonly ISta3020CoReportService _sta3020CoReportService;
    private readonly ISta3080CoReportService _sta3080CoReportService;
    private readonly ISta3071CoReportService _sta3071CoReportService;
    private readonly ISta2020CoReportService _sta2020CoReportService;
    private readonly ISta3010CoReportService _sta3010CoReportService;
    private readonly ISta3030CoReportService _sta3030CoReportService;
    private readonly ISta3001CoReportService _sta3001CoReportService;
    private readonly ISta3040CoReportService _sta3040CoReportService;
    private readonly ISta3041CoReportService _sta3041CoReportService;
    private readonly ISta3050CoReportService _sta3050CoReportService;
    private readonly ISta3060CoReportService _sta3060CoReportService;
    private readonly ISta3061CoReportService _sta3061CoReportService;
    private readonly ISta3070CoReportService _sta3070CoReportService;

    public StatisticService(IDailyStatisticCommandFinder finder, ISta1002CoReportService sta1002CoReportService, ISta1010CoReportService sta1010CoReportService, ISta2001CoReportService sta2001CoReportService, ISta2003CoReportService sta2003CoReportService, ISta1001CoReportService sta1001CoReportService, ISta2002CoReportService sta2002CoReportService, ISta2010CoReportService sta2010CoReportService, ISta2011CoReportService sta2011CoReportService, ISta2021CoReportService sta2021CoReportService, ISta3020CoReportService sta3020CoReportService, ISta3080CoReportService sta3080CoReportService, ISta3071CoReportService sta3071CoReportService, ISta2020CoReportService sta2020CoReportService, ISta3010CoReportService sta3010CoReportService, ISta3030CoReportService sta3030CoReportService, ISta3001CoReportService sta3001CoReportService, ISta3040CoReportService sta3040CoReportService, ISta3041CoReportService sta3041CoReportService, ISta3050CoReportService sta3050CoReportService, ISta3060CoReportService sta3060CoReportService, ISta3061CoReportService sta3061CoReportService, ISta3070CoReportService sta3070CoReportService)
    {
        _finder = finder;
        _sta1002CoReportService = sta1002CoReportService;
        _sta1010CoReportService = sta1010CoReportService;
        _sta2001CoReportService = sta2001CoReportService;
        _sta2003CoReportService = sta2003CoReportService;
        _sta1001CoReportService = sta1001CoReportService;
        _sta2002CoReportService = sta2002CoReportService;
        _sta2010CoReportService = sta2010CoReportService;
        _sta2011CoReportService = sta2011CoReportService;
        _sta2021CoReportService = sta2021CoReportService;
        _sta3020CoReportService = sta3020CoReportService;
        _sta3080CoReportService = sta3080CoReportService;
        _sta3071CoReportService = sta3071CoReportService;
        _sta2020CoReportService = sta2020CoReportService;
        _sta3010CoReportService = sta3010CoReportService;
        _sta3030CoReportService = sta3030CoReportService;
        _sta3001CoReportService = sta3001CoReportService;
        _sta3040CoReportService = sta3040CoReportService;
        _sta3041CoReportService = sta3041CoReportService;
        _sta3050CoReportService = sta3050CoReportService;
        _sta3060CoReportService = sta3060CoReportService;
        _sta3061CoReportService = sta3061CoReportService;
        _sta3070CoReportService = sta3070CoReportService;
    }

    public CommonReportingRequestModel PrintExecute(int hpId, string formName, int menuId, int monthFrom, int monthTo, int dateFrom, int dateTo, int timeFrom, int timeTo, CoFileType? coFileType = null, bool? isPutTotalRow = false, int? tenkiDateFrom = -1, int? tenkiDateTo = -1, int? enableRangeFrom = -1, int? enableRangeTo = -1, long? ptNumFrom = 0, long? ptNumTo = 0)
    {
        var configDaily = _finder.GetDailyConfigStatisticMenu(hpId, menuId);
        CommonReportingRequestModel result = new();
        switch ((StatisticReportType)configDaily.ReportId)
        {
            case StatisticReportType.Sta1001:
                result = PrintSta1001(hpId, configDaily, dateFrom, dateTo, timeFrom, timeTo);
                break;
            case StatisticReportType.Sta1002:
                result = PrintSta1002(hpId, configDaily, dateFrom, dateTo, timeFrom, timeTo);
                break;
            case StatisticReportType.Sta1010:
                result = PrintSta1010(hpId, configDaily, dateFrom, dateTo, timeFrom, timeTo);
                break;
            case StatisticReportType.Sta2001:
                result = PrintSta2001(hpId, configDaily, monthFrom, monthTo);
                break;
            case StatisticReportType.Sta2002:
                result = PrintSta2002(hpId, configDaily, monthFrom, monthTo);
                break;
            case StatisticReportType.Sta2003:
                result = PrintSta2003(hpId, configDaily, monthFrom, monthTo);
                break;
            case StatisticReportType.Sta2010:
                result = PrintSta2010(hpId, configDaily, monthFrom);
                break;
            case StatisticReportType.Sta2011:
                result = PrintSta2011(hpId, configDaily, monthFrom);
                break;
            case StatisticReportType.Sta2021:
                result = PrintSta2021(hpId, configDaily, monthFrom, monthTo);
                break;
            case StatisticReportType.Sta3020:
                result = PrintSta3020(hpId, configDaily, dateFrom);
                break;
            case StatisticReportType.Sta3080:
                result = PrintSta3080(hpId, configDaily, monthFrom, monthTo, coFileType);
                break;
            case StatisticReportType.Sta3071:
                result = PrintSta3071(hpId, configDaily, dateFrom, dateTo, coFileType, isPutTotalRow);
                break;
            case StatisticReportType.Sta2020:
                result = PrintSta2020(hpId, configDaily, timeFrom, timeTo);
                break;
            case StatisticReportType.Sta3010:
                result = PrintSta3010(hpId, configDaily, dateFrom, coFileType);
                break;
            case StatisticReportType.Sta3001:
                result = PrintSta3001(hpId, configDaily, dateFrom);
                break;
            case StatisticReportType.Sta3030:
                result = PrintSta3030(hpId, configDaily, dateFrom, dateTo, tenkiDateFrom ?? -1, tenkiDateTo ?? -1, enableRangeFrom ?? -1, enableRangeTo ?? -1, coFileType);
                break;
            case StatisticReportType.Sta3040:
                result = PrintSta3040(hpId, configDaily, monthFrom, monthTo, coFileType);
                break;
            case StatisticReportType.Sta3041:
                result = PrintSta3041(hpId, configDaily, monthFrom, monthTo, coFileType);
                break;
            case StatisticReportType.Sta3050:
                result = PrintSta3050(hpId, configDaily, dateFrom, dateTo, ptNumFrom ?? 0, ptNumTo ?? 0, coFileType);
                break;
            case StatisticReportType.Sta3060:
                result = PrintSta3060(hpId, configDaily, monthFrom, monthTo, coFileType);
                break;
            case StatisticReportType.Sta3070:
                result = PrintSta3070(hpId, configDaily, monthFrom, monthTo, coFileType);
                break;
            case StatisticReportType.Sta3061:
                result = PrintSta3061(hpId, configDaily, dateFrom, dateTo);
                break;
        }
        result.JobName = configDaily.MenuName;
        return result;
    }

    #region Print Report
    private CommonReportingRequestModel PrintSta1001(int hpId, ConfigStatisticModel configDaily, int dateFrom, int dateTo, int timeFrom, int timeTo)
    {
        CoSta1001PrintConf printConf = CreateCoSta1001PrintConf(configDaily, dateFrom, dateTo, timeFrom, timeTo);
        return _sta1001CoReportService.GetSta1001ReportingData(printConf, hpId);
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

    private CommonReportingRequestModel PrintSta2003(int hpId, ConfigStatisticModel configDaily, int monthFrom, int monthTo)
    {
        var printConf = CreateCoSta2003PrintConf(configDaily, monthFrom, monthTo);
        return _sta2003CoReportService.GetSta2003ReportingData(printConf, hpId);
    }

    private CommonReportingRequestModel PrintSta2010(int hpId, ConfigStatisticModel configDaily, int monthFrom)
    {
        var printConf = CreateCoSta2010PrintConf(configDaily, monthFrom);
        return _sta2010CoReportService.GetSta2010ReportingData(printConf, hpId);
    }

    private CommonReportingRequestModel PrintSta2011(int hpId, ConfigStatisticModel configDaily, int monthFrom)
    {
        var printConf = CreateCoSta2011PrintConf(configDaily, monthFrom);
        return _sta2011CoReportService.GetSta2011ReportingData(printConf, hpId);
    }

    private CommonReportingRequestModel PrintSta2021(int hpId, ConfigStatisticModel configDaily, int monthFrom, int monthTo)
    {
        var printConf = CreateCoSta2021PrintConf(configDaily.ConfigStatistic2021, monthFrom, monthTo);
        return _sta2021CoReportService.GetSta2021ReportingData(printConf, hpId);
    }

    private CommonReportingRequestModel PrintSta3020(int hpId, ConfigStatisticModel configDaily, int dateFrom)
    {
        var printConf = CreateCoSta3020PrintConf(configDaily.ConfigStatistic3020, dateFrom);
        return _sta3020CoReportService.GetSta3020ReportingData(printConf, hpId);
    }

    private CommonReportingRequestModel PrintSta3080(int hpId, ConfigStatisticModel configDaily, int monthFrom, int monthTo, CoFileType? coFileType)
    {
        return _sta3080CoReportService.GetSta3080ReportingData(CreateCoSta3080PrintConf(configDaily.ConfigStatistic3080, monthFrom, monthTo), hpId, coFileType ?? CoFileType.Binary);
    }

    private CommonReportingRequestModel PrintSta3071(int hpId, ConfigStatisticModel configDaily, int dateFrom, int dateTo, CoFileType? coFileType, bool? isPutTotalRow)
    {
        return _sta3071CoReportService.GetSta3071ReportingData(CreateCoSta3071PrintConf(configDaily.ConfigStatistic3071, dateFrom, dateTo), hpId, coFileType ?? CoFileType.Binary, isPutTotalRow ?? false);
    }

    private CommonReportingRequestModel PrintSta2020(int hpId, ConfigStatisticModel configDaily, int timeFrom, int timeTo)
    {
        var printConf = CreateCoSta2020PrintConf(configDaily, timeFrom, timeTo);
        return _sta2020CoReportService.GetSta2020ReportingData(printConf, hpId);
    }

    private CommonReportingRequestModel PrintSta3001(int hpId, ConfigStatisticModel configDaily, int dateFrom)
    {
        var printConf = CreateCoSta3001PrintConf(configDaily.ConfigStatistic3001, dateFrom);
        return _sta3001CoReportService.GetSta3001ReportingData(printConf, hpId);
    }

    private CommonReportingRequestModel PrintSta3010(int hpId, ConfigStatisticModel configDaily, int dateFrom, CoFileType? coFileType)
    {
        return _sta3010CoReportService.GetSta3010ReportingData(CreateCoSta3010PrintConf(configDaily.ConfigStatistic3010, dateFrom), hpId, coFileType ?? CoFileType.Binary);
    }

    private CommonReportingRequestModel PrintSta3030(int hpId, ConfigStatisticModel configDaily, int startDateFrom, int startDateTo, int tenkiDateFrom, int tenkiDateTo, int enableRangeFrom, int enableRangeTo, CoFileType? coFileType)
    {
        return _sta3030CoReportService.GetSta3030ReportingData(CreateCoSta3030PrintConf(configDaily.ConfigStatistic3030, startDateFrom, startDateTo, tenkiDateFrom, tenkiDateTo, enableRangeFrom, enableRangeTo), hpId, coFileType ?? CoFileType.Binary);
    }

    private CommonReportingRequestModel PrintSta3040(int hpId, ConfigStatisticModel configDaily, int monthFrom, int monthTo, CoFileType? coFileType)
    {
        return _sta3040CoReportService.GetSta3040ReportingData(CreateCoSta3040PrintConf(configDaily.ConfigStatistic3040, monthFrom, monthTo), hpId, coFileType ?? CoFileType.Binary);
    }

    private CommonReportingRequestModel PrintSta3041(int hpId, ConfigStatisticModel configDaily, int monthFrom, int monthTo, CoFileType? coFileType)
    {
        return _sta3041CoReportService.GetSta3041ReportingData(CreateCoSta3041PrintConf(configDaily.ConfigStatistic3041, monthFrom, monthTo), hpId, coFileType ?? CoFileType.Binary);
    }

    private CommonReportingRequestModel PrintSta3050(int hpId, ConfigStatisticModel configDaily, int startDateFrom, int startDateTo, long ptNumFrom, long ptNumTo, CoFileType? coFileType)
    {
        return _sta3050CoReportService.GetSta3050ReportingData(CreateCoSta3050PrintConf(configDaily.ConfigStatistic3050, startDateFrom, startDateTo, ptNumFrom, ptNumTo), hpId, coFileType ?? CoFileType.Binary);
    }

    private CommonReportingRequestModel PrintSta3060(int hpId, ConfigStatisticModel configDaily, int monthFrom, int monthTo, CoFileType? coFileType)
    {
        return _sta3060CoReportService.GetSta3060ReportingData(CreateCoSta3060PrintConf(configDaily.ConfigStatistic3060, monthFrom, monthTo), hpId, coFileType ?? CoFileType.Binary);
    }

    private CommonReportingRequestModel PrintSta3070(int hpId, ConfigStatisticModel configDaily, int monthFrom, int monthTo, CoFileType? coFileType)
    {
        return _sta3070CoReportService.GetSta3070ReportingData(CreateCoSta3070PrintConf(configDaily.ConfigStatistic3070, monthFrom, monthTo), hpId, coFileType ?? CoFileType.Binary);
    }

    private CommonReportingRequestModel PrintSta3061(int hpId, ConfigStatisticModel configDaily, int startDateFrom, int startDateTo)
    {
        return _sta3061CoReportService.GetSta3061ReportingData(CreateCoSta3061PrintConf(configDaily.ConfigStatistic3061, startDateFrom, startDateTo), hpId);
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

    private CoSta2003PrintConf CreateCoSta2003PrintConf(ConfigStatisticModel configDaily,
                                                                 int monthFrom,
                                                                 int monthTo)

    {
        CoSta2003PrintConf printConf = new CoSta2003PrintConf(configDaily.MenuId);
        printConf.StartNyukinYm = monthFrom;
        printConf.EndNyukinYm = monthTo;
        printConf.FormFileName = configDaily.FormReport;
        printConf.ReportName = configDaily.ReportName;
        printConf.PageBreak1 = configDaily.BreakPage1;
        printConf.PageBreak2 = configDaily.BreakPage2;
        printConf.SortOrder1 = configDaily.SortOrder1;
        printConf.SortOpt1 = configDaily.OrderBy1;
        printConf.IsTester = configDaily.TestPatient == 1;
        printConf.IsExcludeUnpaid = configDaily.ExcludingUnpaid == 1;
        printConf.KaIds = configDaily.KaId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.TantoIds = configDaily.UserId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.HokenSbts = configDaily.InsuranceType.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.IsTensu = configDaily.MedicalTreatment;
        printConf.IsJihiFutan = configDaily.NonInsuranceAmount;
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

    private CoSta2010PrintConf CreateCoSta2010PrintConf(ConfigStatisticModel configDaily, int monthFrom)
    {
        CoSta2010PrintConf printConf = new CoSta2010PrintConf(configDaily.MenuId);
        printConf.SeikyuYm = monthFrom;
        printConf.FormFileName = configDaily.FormReport;
        printConf.ReportName = configDaily.ReportName;
        printConf.PageBreak1 = configDaily.BreakPage1;
        printConf.PageBreak2 = configDaily.BreakPage2;
        printConf.IsTester = configDaily.TestPatient == 1;
        printConf.MainHokensyaNo = configDaily.Designated == 1;
        printConf.KaIds = configDaily.KaId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.TantoIds = configDaily.UserId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.HokenSbts = configDaily.InsuranceType.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.SeikyuTypes = configDaily.TargetReceipt.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();

        return printConf;
    }

    private CoSta2011PrintConf CreateCoSta2011PrintConf(ConfigStatisticModel configDaily, int monthFrom)

    {
        CoSta2011PrintConf printConf = new CoSta2011PrintConf(configDaily.MenuId);
        printConf.SeikyuYm = monthFrom;
        printConf.FormFileName = configDaily.FormReport;
        printConf.ReportName = configDaily.ReportName;
        printConf.PageBreak1 = configDaily.BreakPage1;
        printConf.PageBreak2 = configDaily.BreakPage2;
        printConf.PageBreak3 = configDaily.BreakPage3;
        printConf.IsTester = configDaily.TestPatient == 1;
        printConf.KaIds = configDaily.KaId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.TantoIds = configDaily.UserId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.SeikyuTypes = configDaily.TargetReceipt.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.IsZaitaku = configDaily.HomePatient == 1;
        printConf.IsUchiwake = configDaily.ShowBreakdown == 1;
        printConf.ZaitakuItems = configDaily.ItemInput.Split(' ').Where(x => !string.IsNullOrEmpty(x)).ToList();

        return printConf;
    }

    private CoSta2021PrintConf CreateCoSta2021PrintConf(ConfigStatistic2021Model configStatistic, int monthFrom, int monthTo)
    {
        CoSta2021PrintConf printConf = new CoSta2021PrintConf(configStatistic.MenuId);
        printConf.FormFileName = configStatistic.FormReport;
        printConf.ReportName = configStatistic.ReportName;
        printConf.IsTester = configStatistic.TestPatient == 1;

        printConf.StartSinYm = monthFrom;
        printConf.EndSinYm = monthTo;

        printConf.DataKind = configStatistic.DataKind;
        printConf.PageBreak1 = configStatistic.PageBreak1;
        printConf.PageBreak2 = configStatistic.PageBreak2;
        printConf.SortOrder1 = configStatistic.SortOrder1;
        printConf.SortOpt1 = configStatistic.SortOpt1;
        printConf.SortOrder2 = configStatistic.SortOrder2;
        printConf.SortOpt2 = configStatistic.SortOpt2;
        printConf.SortOrder3 = configStatistic.SortOrder3;
        printConf.SortOpt3 = configStatistic.SortOpt3;
        printConf.KaIds = configStatistic.ListKaId;
        printConf.TantoIds = configStatistic.ListTantoId;
        printConf.HokenSbts = configStatistic.ListHokenSbt;
        printConf.SinIds = configStatistic.ListSinId;
        printConf.SinKouiKbns = configStatistic.ListSinKouiKbn;
        printConf.MadokuKbns = configStatistic.ListMadokuKbn;
        printConf.KouseisinKbns = configStatistic.ListKouseisinKbn;
        printConf.ItemCds = configStatistic.ListItemCd;
        printConf.SearchWord = configStatistic.SearchWord;
        printConf.SearchOpt = configStatistic.SearchOpt;
        printConf.ItemSearchOpt = configStatistic.ItemCdOpt;
        printConf.InoutKbns = configStatistic.ListInoutKbn;
        printConf.KohatuKbns = configStatistic.ListKohatuKbn;
        printConf.IsAdopteds = configStatistic.ListIsAdopted;

        return printConf;
    }

    private CoSta3020PrintConf CreateCoSta3020PrintConf(ConfigStatistic3020Model configStatistic, int stdDate)
    {
        CoSta3020PrintConf printConf = new CoSta3020PrintConf(configStatistic.MenuId);
        printConf.StdDate = stdDate;
        printConf.FormFileName = configStatistic.FormReport;
        printConf.ReportName = configStatistic.ReportName;
        printConf.PageBreak1 = configStatistic.PageBreak1;

        printConf.SetKbnKanri = configStatistic.SetKbnKanri > 0;
        printConf.SetKbnZaitaku = configStatistic.SetKbnZaitaku > 0;
        printConf.SetKbnSyoho = configStatistic.SetKbnSyoho > 0;
        printConf.SetKbnYoho = configStatistic.SetKbnYoho > 0;
        printConf.SetKbnChusyaSyugi = configStatistic.SetKbnChusyaSyugi > 0;
        printConf.SetKbnChusya = configStatistic.SetKbnChusya > 0;
        printConf.SetKbnSyochi = configStatistic.SetKbnSyochi > 0;
        printConf.SetKbnKensa = configStatistic.SetKbnKensa > 0;
        printConf.SetKbnSyujutsu = configStatistic.SetKbnSyujutsu > 0;
        printConf.SetKbnGazo = configStatistic.SetKbnGazo > 0;
        printConf.SetKbnSonota = configStatistic.SetKbnSonota > 0;
        printConf.SetKbnJihi = configStatistic.SetKbnJihi > 0;
        printConf.SetKbnByomei = configStatistic.SetKbnByomei > 0;

        printConf.TgtData = configStatistic.TargetData;

        printConf.SearchWord = configStatistic.SearchWord;
        printConf.SearchOpt = configStatistic.SearchOpt;

        printConf.ItemSearchOpt = configStatistic.ItemSearchOpt;
        if (configStatistic.ItemSearchOpt == 0)
        {
            printConf.ItemCds = configStatistic.ListItemCd;
        }
        else
        {
            printConf.ItemCds = configStatistic.ListByomeiCd;
        }

        return printConf;
    }

    private CoSta2020PrintConf CreateCoSta2020PrintConf(ConfigStatisticModel configDaily, int dateFrom, int dateTo)

    {
        CoSta2020PrintConf printConf = new CoSta2020PrintConf(configDaily.MenuId);
        if (dateFrom.AsString().Length == 6)
        {
            printConf.StartSinYm = dateFrom;
            printConf.EndSinYm = dateTo;
        }
        else if (dateFrom.AsString().Length == 8)
        {
            printConf.StartSinDate = dateFrom;
            printConf.EndSinDate = dateTo;
        }

        printConf.FormFileName = configDaily.FormReport;
        printConf.ReportName = configDaily.ReportName;
        printConf.DataKind = configDaily.TargetData;
        printConf.PageBreak1 = configDaily.BreakPage1;
        printConf.PageBreak2 = configDaily.BreakPage2;
        printConf.PageBreak3 = configDaily.BreakPage3;
        printConf.SortOrder1 = configDaily.SortOrder1;
        printConf.SortOpt1 = configDaily.OrderBy1;
        printConf.SortOrder2 = configDaily.SortOrder2;
        printConf.SortOpt2 = configDaily.OrderBy2;
        printConf.SortOrder3 = configDaily.SortOrder3;
        printConf.SortOpt3 = configDaily.OrderBy3;
        printConf.IsTester = configDaily.TestPatient == 1;
        printConf.KaIds = configDaily.KaId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.TantoIds = configDaily.UserId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.HokenSbts = configDaily.InsuranceType.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.SinIds = configDaily.MedicaIdentification.Split(' ').Where(x => !string.IsNullOrEmpty(x)).ToList();
        printConf.SinKouiKbns = configDaily.DiagnosisTreatment.Split(' ').Where(x => !string.IsNullOrEmpty(x)).ToList();
        printConf.MadokuKbns = configDaily.Leprosy.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.KouseisinKbns = configDaily.PsychotropiDrug.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.SearchWord = configDaily.KeySearch;
        printConf.SearchOpt = configDaily.SearchOperator;
        printConf.ItemCds = configDaily.ItemInput.Split(' ').Where(x => !string.IsNullOrEmpty(x)).ToList();
        printConf.ItemSearchOpt = configDaily.ItemCdOpt;
        printConf.InoutKbns = configDaily.ListInoutKbn;
        printConf.KohatuKbns = configDaily.ListKohatuKbn;
        printConf.IsAdopteds = configDaily.ListIsAdopted;

        return printConf;
    }

    private CoSta3080PrintConf CreateCoSta3080PrintConf(ConfigStatistic3080Model configStatistic, int timeFrom, int timeTo)
    {
        CoSta3080PrintConf printConf = new CoSta3080PrintConf(configStatistic.MenuId);
        printConf.FormFileName = configStatistic.FormReport;
        printConf.ReportName = configStatistic.ReportName;
        printConf.IsTester = configStatistic.TestPatient == 1;
        printConf.FromYm = timeFrom;
        printConf.ToYm = timeTo;
        return printConf;
    }

    private CoSta3010PrintConf CreateCoSta3010PrintConf(ConfigStatistic3010Model configStatistic, int stdDate)
    {
        CoSta3010PrintConf printConf = new CoSta3010PrintConf(configStatistic.MenuId);
        int[] arrEdaNo = { 0, 1, 2, 3, 4, 5 };

        // 帳票タイトル
        printConf.ReportName = configStatistic.ReportName;

        // フォームファイル名
        printConf.FormFileName = configStatistic.FormReport;

        // 基準日
        printConf.StdDate = stdDate;

        // 改ページ１
        printConf.PageBreak1 = configStatistic.BreakPage1;

        // セット区分１_1~6
        int lengthSet1 = configStatistic.Set1_1 +
                         configStatistic.Set1_2 +
                         configStatistic.Set1_3 +
                         configStatistic.Set1_4 +
                         configStatistic.Set1_5 +
                         configStatistic.Set1_6;
        if (configStatistic.Set1 == 1 || lengthSet1 > 0)
        {
            printConf.Set1 = arrEdaNo.ToList();
            bool isRemove = 0 < lengthSet1 && lengthSet1 < 6;

            if (configStatistic.Set1_1 == 0 && isRemove)
            {
                printConf.Set1.Remove(0);
            }

            if (configStatistic.Set1_2 == 0 && isRemove)
            {
                printConf.Set1.Remove(1);
            }

            if (configStatistic.Set1_3 == 0 && isRemove)
            {
                printConf.Set1.Remove(2);
            }

            if (configStatistic.Set1_4 == 0 && isRemove)
            {
                printConf.Set1.Remove(3);
            }

            if (configStatistic.Set1_5 == 0 && isRemove)
            {
                printConf.Set1.Remove(4);
            }

            if (configStatistic.Set1_6 == 0 && isRemove)
            {
                printConf.Set1.Remove(5);
            }
        }

        // セット区分2_1~6
        int lengthSet2 = configStatistic.Set2_1 +
                         configStatistic.Set2_2 +
                         configStatistic.Set2_3 +
                         configStatistic.Set2_4 +
                         configStatistic.Set2_5 +
                         configStatistic.Set2_6;
        if (configStatistic.Set2 == 1 || lengthSet2 > 0)
        {
            printConf.Set2 = arrEdaNo.ToList();
            bool isRemove = 0 < lengthSet2 && lengthSet2 < 6;

            if (configStatistic.Set2_1 == 0 && isRemove)
            {
                printConf.Set2.Remove(0);
            }

            if (configStatistic.Set2_2 == 0 && isRemove)
            {
                printConf.Set2.Remove(1);
            }

            if (configStatistic.Set2_3 == 0 && isRemove)
            {
                printConf.Set2.Remove(2);
            }

            if (configStatistic.Set2_4 == 0 && isRemove)
            {
                printConf.Set2.Remove(3);
            }

            if (configStatistic.Set2_5 == 0 && isRemove)
            {
                printConf.Set2.Remove(4);
            }

            if (configStatistic.Set2_6 == 0 && isRemove)
            {
                printConf.Set2.Remove(5);
            }
        }

        // セット区分3_1~6
        int lengthSet3 = configStatistic.Set3_1 +
                         configStatistic.Set3_2 +
                         configStatistic.Set3_3 +
                         configStatistic.Set3_4 +
                         configStatistic.Set3_5 +
                         configStatistic.Set3_6;
        if (configStatistic.Set3 == 1 || lengthSet3 > 0)
        {
            printConf.Set3 = arrEdaNo.ToList();
            bool isRemove = 0 < lengthSet3 && lengthSet3 < 6;

            if (configStatistic.Set3_1 == 0 && isRemove)
            {
                printConf.Set3.Remove(0);
            }

            if (configStatistic.Set3_2 == 0 && isRemove)
            {
                printConf.Set3.Remove(1);
            }

            if (configStatistic.Set3_3 == 0 && isRemove)
            {
                printConf.Set3.Remove(2);
            }

            if (configStatistic.Set3_4 == 0 && isRemove)
            {
                printConf.Set3.Remove(3);
            }

            if (configStatistic.Set3_5 == 0 && isRemove)
            {
                printConf.Set3.Remove(4);
            }

            if (configStatistic.Set3_6 == 0 && isRemove)
            {
                printConf.Set3.Remove(5);
            }
        }

        // セット区分4_1~6
        int lengthSet4 = configStatistic.Set4_1 +
                         configStatistic.Set4_2 +
                         configStatistic.Set4_3 +
                         configStatistic.Set4_4 +
                         configStatistic.Set4_5 +
                         configStatistic.Set4_6;
        if (configStatistic.Set4 == 1 || lengthSet4 > 0)
        {
            printConf.Set4 = arrEdaNo.ToList();
            bool isRemove = 0 < lengthSet4 && lengthSet4 < 6;

            if (configStatistic.Set4_1 == 0 && isRemove)
            {
                printConf.Set4.Remove(0);
            }

            if (configStatistic.Set4_2 == 0 && isRemove)
            {
                printConf.Set4.Remove(1);
            }

            if (configStatistic.Set4_3 == 0 && isRemove)
            {
                printConf.Set4.Remove(2);
            }

            if (configStatistic.Set4_4 == 0 && isRemove)
            {
                printConf.Set4.Remove(3);
            }

            if (configStatistic.Set4_5 == 0 && isRemove)
            {
                printConf.Set4.Remove(4);
            }

            if (configStatistic.Set4_6 == 0 && isRemove)
            {
                printConf.Set4.Remove(5);
            }
        }

        // セット区分5_1~6
        int lengthSet5 = configStatistic.Set5_1 +
                         configStatistic.Set5_2 +
                         configStatistic.Set5_3 +
                         configStatistic.Set5_4 +
                         configStatistic.Set5_5 +
                         configStatistic.Set5_6;
        if (configStatistic.Set5 == 1 || lengthSet5 > 0)
        {
            printConf.Set5 = arrEdaNo.ToList();
            bool isRemove = 0 < lengthSet5 && lengthSet5 < 6;

            if (configStatistic.Set5_1 == 0 && isRemove)
            {
                printConf.Set5.Remove(0);
            }

            if (configStatistic.Set5_2 == 0 && isRemove)
            {
                printConf.Set5.Remove(1);
            }

            if (configStatistic.Set5_3 == 0 && isRemove)
            {
                printConf.Set5.Remove(2);
            }

            if (configStatistic.Set5_4 == 0 && isRemove)
            {
                printConf.Set5.Remove(3);
            }

            if (configStatistic.Set5_5 == 0 && isRemove)
            {
                printConf.Set5.Remove(4);
            }

            if (configStatistic.Set5_6 == 0 && isRemove)
            {
                printConf.Set5.Remove(5);
            }
        }

        // セット区分6_1~6
        int lengthSet6 = configStatistic.Set6_1 +
                         configStatistic.Set6_2 +
                         configStatistic.Set6_3 +
                         configStatistic.Set6_4 +
                         configStatistic.Set6_5 +
                         configStatistic.Set6_6;
        if (configStatistic.Set6 == 1 || lengthSet6 > 0)
        {
            printConf.Set6 = arrEdaNo.ToList();
            bool isRemove = 0 < lengthSet6 && lengthSet6 < 6;

            if (configStatistic.Set6_1 == 0 && isRemove)
            {
                printConf.Set6.Remove(0);
            }

            if (configStatistic.Set6_2 == 0 && isRemove)
            {
                printConf.Set6.Remove(1);
            }

            if (configStatistic.Set6_3 == 0 && isRemove)
            {
                printConf.Set6.Remove(2);
            }

            if (configStatistic.Set6_4 == 0 && isRemove)
            {
                printConf.Set6.Remove(3);
            }

            if (configStatistic.Set6_5 == 0 && isRemove)
            {
                printConf.Set6.Remove(4);
            }

            if (configStatistic.Set6_6 == 0 && isRemove)
            {
                printConf.Set6.Remove(5);
            }
        }

        // セット区分7_1~6
        int lengthSet7 = configStatistic.Set7_1 +
                         configStatistic.Set7_2 +
                         configStatistic.Set7_3 +
                         configStatistic.Set7_4 +
                         configStatistic.Set7_5 +
                         configStatistic.Set7_6;
        if (configStatistic.Set7 == 1 || lengthSet7 > 0)
        {
            printConf.Set7 = arrEdaNo.ToList();
            bool isRemove = 0 < lengthSet7 && lengthSet7 < 6;

            if (configStatistic.Set7_1 == 0 && isRemove)
            {
                printConf.Set7.Remove(0);
            }

            if (configStatistic.Set7_2 == 0 && isRemove)
            {
                printConf.Set7.Remove(1);
            }

            if (configStatistic.Set7_3 == 0 && isRemove)
            {
                printConf.Set7.Remove(2);
            }

            if (configStatistic.Set7_4 == 0 && isRemove)
            {
                printConf.Set7.Remove(3);
            }

            if (configStatistic.Set7_5 == 0 && isRemove)
            {
                printConf.Set7.Remove(4);
            }

            if (configStatistic.Set7_6 == 0 && isRemove)
            {
                printConf.Set7.Remove(5);
            }
        }

        // セット区分8_1~6
        int lengthSet8 = configStatistic.Set8_1 +
                         configStatistic.Set8_2 +
                         configStatistic.Set8_3 +
                         configStatistic.Set8_4 +
                         configStatistic.Set8_5 +
                         configStatistic.Set8_6;
        if (configStatistic.Set8 == 1 || lengthSet8 > 0)
        {
            printConf.Set8 = arrEdaNo.ToList();
            bool isRemove = 0 < lengthSet8 && lengthSet8 < 6;

            if (configStatistic.Set8_1 == 0 && isRemove)
            {
                printConf.Set8.Remove(0);
            }

            if (configStatistic.Set8_2 == 0 && isRemove)
            {
                printConf.Set8.Remove(1);
            }

            if (configStatistic.Set8_3 == 0 && isRemove)
            {
                printConf.Set8.Remove(2);
            }

            if (configStatistic.Set8_4 == 0 && isRemove)
            {
                printConf.Set8.Remove(3);
            }

            if (configStatistic.Set8_5 == 0 && isRemove)
            {
                printConf.Set8.Remove(4);
            }

            if (configStatistic.Set8_6 == 0 && isRemove)
            {
                printConf.Set8.Remove(5);
            }
        }

        // セット区分9_1~6
        int lengthSet9 = configStatistic.Set9_1 +
                         configStatistic.Set9_2 +
                         configStatistic.Set9_3 +
                         configStatistic.Set9_4 +
                         configStatistic.Set9_5 +
                         configStatistic.Set9_6;
        if (configStatistic.Set9 == 1 || lengthSet9 > 0)
        {
            printConf.Set9 = arrEdaNo.ToList();
            bool isRemove = 0 < lengthSet9 && lengthSet9 < 6;

            if (configStatistic.Set9_1 == 0 && isRemove)
            {
                printConf.Set9.Remove(0);
            }

            if (configStatistic.Set9_2 == 0 && isRemove)
            {
                printConf.Set9.Remove(1);
            }

            if (configStatistic.Set9_3 == 0 && isRemove)
            {
                printConf.Set9.Remove(2);
            }

            if (configStatistic.Set9_4 == 0 && isRemove)
            {
                printConf.Set9.Remove(3);
            }

            if (configStatistic.Set9_5 == 0 && isRemove)
            {
                printConf.Set9.Remove(4);
            }

            if (configStatistic.Set9_6 == 0 && isRemove)
            {
                printConf.Set9.Remove(5);
            }
        }

        // セット区分10_1~6
        int lengthSet10 = configStatistic.Set10_1 +
                          configStatistic.Set10_2 +
                          configStatistic.Set10_3 +
                          configStatistic.Set10_4 +
                          configStatistic.Set10_5 +
                          configStatistic.Set10_6;
        if (configStatistic.Set10 == 1 || lengthSet10 > 0)
        {
            printConf.Set10 = arrEdaNo.ToList();
            bool isRemove = 0 < lengthSet10 && lengthSet10 < 6;

            if (configStatistic.Set10_1 == 0 && isRemove)
            {
                printConf.Set10.Remove(0);
            }

            if (configStatistic.Set10_2 == 0 && isRemove)
            {
                printConf.Set10.Remove(1);
            }

            if (configStatistic.Set10_3 == 0 && isRemove)
            {
                printConf.Set10.Remove(2);
            }

            if (configStatistic.Set10_4 == 0 && isRemove)
            {
                printConf.Set10.Remove(3);
            }

            if (configStatistic.Set10_5 == 0 && isRemove)
            {
                printConf.Set10.Remove(4);
            }

            if (configStatistic.Set10_6 == 0 && isRemove)
            {
                printConf.Set10.Remove(5);
            }
        }

        // 対象データ
        printConf.TgtData = configStatistic.TargetData;

        // セット内の他の項目
        printConf.OtherItemOpt = configStatistic.OtherItemOpt;

        // 検索項目
        printConf.ItemCds = configStatistic.GetListItemCd();
        printConf.ItemSearchOpt = configStatistic.ItemCdOpt;

        // 検索ワード
        printConf.SearchWord = configStatistic.SearchWord;

        // 検索ワードの検索オプション
        printConf.SearchOpt = configStatistic.SearchOpt;

        return printConf;
    }

    private CoSta3001PrintConf CreateCoSta3001PrintConf(ConfigStatistic3001Model configStatistic, int stdDate)
    {
        CoSta3001PrintConf printConf = new CoSta3001PrintConf(configStatistic.MenuId);
        printConf.StdDate = stdDate;
        printConf.ReportName = configStatistic.ReportName;
        printConf.FormFileName = configStatistic.FormReport;
        printConf.PageBreak1 = configStatistic.BreakPage1;
        printConf.SortOrder1 = configStatistic.SortOrder1;
        printConf.SortOrder2 = configStatistic.SortOrder2;
        printConf.SortOrder3 = configStatistic.SortOrder3;
        printConf.SortOpt1 = configStatistic.OrderBy1;
        printConf.SortOpt2 = configStatistic.OrderBy2;
        printConf.SortOpt3 = configStatistic.OrderBy3;

        if (configStatistic.StartDateFrom > 0)
        {
            printConf.StartDateFrom = configStatistic.StartDateFrom;
        }
        if (configStatistic.StartDateTo > 0)
        {
            printConf.StartDateTo = configStatistic.StartDateTo;
        }

        if (configStatistic.EndDateFrom > 0)
        {
            printConf.EndDateFrom = configStatistic.EndDateFrom;
        }
        if (configStatistic.EndDateTo > 0)
        {
            printConf.EndDateTo = configStatistic.EndDateTo;
        }

        printConf.IpnNameOpt = configStatistic.OptionCommonName;
        printConf.ReceNameOpt = configStatistic.OptionReceiptName;
        printConf.DrugKbns = new List<int>();

        // 内用薬
        if (configStatistic.DrugCategoryInternal == 1)
        {
            printConf.DrugKbns.Add(1);
        }

        // その他
        if (configStatistic.DrugCategoryOther == 1)
        {
            printConf.DrugKbns.Add(3);
        }

        // 注射薬
        if (configStatistic.DrugCategoryInjection == 1)
        {
            printConf.DrugKbns.Add(4);
        }

        // 外用薬
        if (configStatistic.DrugCategoryTopical == 1)
        {
            printConf.DrugKbns.Add(6);
        }

        // 歯科用薬剤
        if (configStatistic.DrugCategoryDental == 1)
        {
            printConf.DrugKbns.Add(8);
        }

        printConf.MadokuKbns = new List<int>();
        //麻毒等以外
        if (configStatistic.MalignantCategoryOtherThanNarcotic == 1)
        {
            printConf.MadokuKbns.Add(0);
        }

        //麻薬
        if (configStatistic.MalignantCategoryNarcotic == 1)
        {
            printConf.MadokuKbns.Add(1);
        }

        //毒薬
        if (configStatistic.MalignantCategorynPoisonousDrug == 1)
        {
            printConf.MadokuKbns.Add(2);
        }

        //覚せい剤
        if (configStatistic.MalignantCategoryAntipsychotics == 1)
        {
            printConf.MadokuKbns.Add(3);
        }

        //向精神薬
        if (configStatistic.MalignantCategoryPsychotropicDrug == 1)
        {
            printConf.MadokuKbns.Add(5);
        }

        printConf.KouseisinKbns = new List<int>();
        //向精神薬以外
        if (configStatistic.PsychotropicDrugCategoryOtherThanPsychotropicDrugs == 1)
        {
            printConf.KouseisinKbns.Add(0);
        }

        //抗不安薬
        if (configStatistic.PsychotropicDrugCategoryAnxiolytics == 1)
        {
            printConf.KouseisinKbns.Add(1);
        }

        //睡眠薬
        if (configStatistic.PsychotropicDrugCategorySleepingPills == 1)
        {
            printConf.KouseisinKbns.Add(2);
        }

        //抗うつ薬
        if (configStatistic.PsychotropicDrugCategoryAntidepressants == 1)
        {
            printConf.KouseisinKbns.Add(3);
        }

        //抗精神病薬
        if (configStatistic.PsychotropicDrugCategoryAntipsychoticDrugs == 1)
        {
            printConf.KouseisinKbns.Add(4);
        }

        printConf.KohatuKbns = new List<int>();
        //先発品 （後発品なし）
        if (configStatistic.GeneralFlagOriginalProductNoGeneric == 1)
        {
            printConf.KohatuKbns.Add(0);
        }

        //先発品（後発品あり）
        if (configStatistic.GeneralFlagOriginalProductWithGeneric == 1)
        {
            printConf.KohatuKbns.Add(2);
        }

        //後発品
        if (configStatistic.GeneralFlagGeneralProcduct == 1)
        {
            printConf.KohatuKbns.Add(1);
        }

        return printConf;
    }

    private CoSta3071PrintConf CreateCoSta3071PrintConf(ConfigStatistic3071Model configStatistic, int timeFrom, int timeTo)
    {
        CoSta3071PrintConf printConf = new CoSta3071PrintConf(configStatistic.MenuId);
        printConf.FormFileName = configStatistic.FormReport;
        printConf.ReportName = configStatistic.ReportName;
        printConf.ReportKbnV = configStatistic.ReportKbnV;
        printConf.ReportKbnH = configStatistic.ReportKbnH;
        printConf.IsTester = configStatistic.TestPatient == 1;
        printConf.RangeFrom = timeFrom;
        printConf.RangeTo = timeTo;

        printConf.KaIds = configStatistic.ListKaId;
        printConf.TantoIds = configStatistic.ListTantoId;
        return printConf;
    }

    private CoSta3030PrintConf CreateCoSta3030PrintConf(ConfigStatistic3030Model configStatistic, int startDateFrom, int startDateTo, int tenkiDateFrom, int tenkiDateTo, int enableRangeFrom, int enableRangeTo)
    {
        CoSta3030PrintConf printConf = new CoSta3030PrintConf(configStatistic.MenuId);
        printConf.FormFileName = configStatistic.FormReport;
        printConf.ReportName = configStatistic.ReportName;

        printConf.PageBreak1 = configStatistic.PageBreak1;
        printConf.SortOrder1 = configStatistic.SortOrder1;
        printConf.SortOpt1 = configStatistic.SortOpt1;
        printConf.SortOrder2 = configStatistic.SortOrder2;
        printConf.SortOpt2 = configStatistic.SortOpt2;
        printConf.SortOrder3 = configStatistic.SortOrder3;
        printConf.SortOpt3 = configStatistic.SortOpt3;
        printConf.IsTester = configStatistic.IsIncludeTestPt;
        if (startDateFrom > 0)
        {
            printConf.StartDateFrom = startDateFrom;
        }

        if (startDateTo > 0)
        {
            printConf.StartDateTo = startDateTo;
        }

        if (tenkiDateFrom > 0)
        {
            printConf.TenkiDateFrom = tenkiDateFrom;
        }

        if (tenkiDateTo > 0)
        {
            printConf.TenkiDateTo = tenkiDateTo;
        }

        if (enableRangeFrom > 0)
        {
            printConf.EnableRangeFrom = enableRangeFrom;
        }

        if (enableRangeTo > 0)
        {
            printConf.EnableRangeTo = enableRangeTo;
        }

        printConf.TenkiKbns = configStatistic.ListTenkiKbn;
        printConf.SyubyoKbns = configStatistic.ListSyubyo;
        printConf.DoubtKbns = configStatistic.ListDoubt;
        printConf.PtIds = configStatistic.ListPtId;

        if (configStatistic.SinDateFrom > 0)
        {
            printConf.SinDateFrom = configStatistic.SinDateFrom;
        }

        if (configStatistic.SinDateTo > 0)
        {
            printConf.SinDateTo = configStatistic.SinDateTo;
        }

        printConf.ByomeiWordOpt = configStatistic.ByomeiWordOpt;
        printConf.ByomeiWords = configStatistic.ByomeiWords;

        printConf.ByomeiCdOpt = configStatistic.ByomeiCdOpt;
        printConf.ByomeiCds = configStatistic.ListByomeiCd;
        printConf.FreeByomeis = configStatistic.ListFreeByomei;

        printConf.SanteiOrder = configStatistic.SanteiOrder;

        printConf.ItemWordOpt = configStatistic.ItemWordOpt;
        printConf.ItemWords = configStatistic.ItemWords;

        printConf.ItemCdOpt = configStatistic.ItemCdOpt;
        printConf.ItemCds = configStatistic.ListItemCd;
        return printConf;
    }

    private CoSta3040PrintConf CreateCoSta3040PrintConf(ConfigStatistic3040Model configStatistic, int monthFrom, int monthTo)
    {
        CoSta3040PrintConf printConf = new CoSta3040PrintConf(configStatistic.MenuId);
        printConf.FromYm = monthFrom;
        printConf.ToYm = monthTo;
        printConf.ReportName = configStatistic.ReportName;
        printConf.FormFileName = configStatistic.FormReport;
        printConf.SortOrder1 = configStatistic.SortOrder1;
        printConf.SortOpt1 = configStatistic.OrderBy1;
        printConf.SortOrder2 = configStatistic.SortOrder2;
        printConf.SortOpt2 = configStatistic.OrderBy2;
        printConf.IsTester = configStatistic.TestPatient == 1;
        printConf.SinryoSbt = configStatistic.SinryoSbt;
        return printConf;
    }

    private CoSta3041PrintConf CreateCoSta3041PrintConf(ConfigStatistic3041Model configStatistic, int monthFrom, int monthTo)
    {
        CoSta3041PrintConf printConf = new CoSta3041PrintConf(configStatistic.MenuId);
        printConf.FromYm = monthFrom;
        printConf.ToYm = monthTo;
        printConf.ReportName = configStatistic.ReportName;
        printConf.FormFileName = configStatistic.FormReport;
        printConf.IsTester = configStatistic.TestPatient == 1;
        return printConf;
    }

    private CoSta3050PrintConf CreateCoSta3050PrintConf(ConfigStatistic3050Model configStatistic, int dateMonthFrom, int dateMonthTo, long ptNumFrom, long ptNumTo)
    {
        CoSta3050PrintConf printConf = new CoSta3050PrintConf(configStatistic.MenuId);
        printConf.FormFileName = configStatistic.FormReport;
        printConf.ReportName = configStatistic.ReportName;
        printConf.IsTester = configStatistic.TestPatient == 1;

        if (dateMonthFrom.AsString().Length == 8 && dateMonthTo.AsString().Length == 8)
        {
            printConf.StartSinDate = dateMonthFrom;
            printConf.EndSinDate = dateMonthTo;
        }

        if (dateMonthFrom.AsString().Length == 6 && dateMonthTo.AsString().Length == 6)
        {
            printConf.StartSinYm = dateMonthFrom;
            printConf.EndSinYm = dateMonthTo;
        }

        if (ptNumFrom > 0)
        {
            printConf.StartPtNum = ptNumFrom;
        }

        if (ptNumTo > 0)
        {
            printConf.EndPtNum = ptNumTo;
        }

        printConf.DataKind = configStatistic.DataKind;
        printConf.PageBreak1 = configStatistic.PageBreak1;
        printConf.PageBreak2 = configStatistic.PageBreak2;
        printConf.PageBreak3 = configStatistic.PageBreak3;
        printConf.SortOrder1 = configStatistic.SortOrder1;
        printConf.SortOpt1 = configStatistic.SortOpt1;
        printConf.SortOrder2 = configStatistic.SortOrder2;
        printConf.SortOpt2 = configStatistic.SortOpt2;
        printConf.SortOrder3 = configStatistic.SortOrder3;
        printConf.SortOpt3 = configStatistic.SortOpt3;
        printConf.KaIds = configStatistic.ListKaId;
        printConf.TantoIds = configStatistic.ListTantoId;
        printConf.HokenSbts = configStatistic.ListHokenSbt;
        printConf.SinIds = configStatistic.ListSinId;
        printConf.SinKouiKbns = configStatistic.ListSinKouiKbn;
        printConf.InoutKbns = configStatistic.ListInoutKbn;
        printConf.MadokuKbns = configStatistic.ListMadokuKbn;
        printConf.KouseisinKbns = configStatistic.ListKouseisinKbn;
        printConf.KohatuKbns = configStatistic.ListKohatuKbn;
        printConf.IsAdopteds = configStatistic.ListIsAdopted;
        printConf.ItemCds = configStatistic.ListItemCd;
        printConf.ItemSearchOpt = configStatistic.ItemCdOpt;
        printConf.SearchWord = configStatistic.SearchWord;
        printConf.SearchOpt = configStatistic.SearchOpt;

        return printConf;
    }

    private CoSta3060PrintConf CreateCoSta3060PrintConf(ConfigStatistic3060Model configStatistic, int monthFrom, int monthTo)
    {
        CoSta3060PrintConf printConf = new CoSta3060PrintConf(configStatistic.MenuId);
        printConf.FormFileName = configStatistic.FormReport;
        printConf.ReportName = configStatistic.ReportName;
        printConf.ReportKbn = configStatistic.ReportKbn;
        printConf.IsTester = configStatistic.TestPatient == 1;
        printConf.StartSinYm = monthFrom;
        printConf.EndSinYm = monthTo;

        printConf.PageBreak1 = configStatistic.PageBreak1;
        printConf.PageBreak2 = configStatistic.PageBreak2;
        printConf.PageBreak3 = configStatistic.PageBreak3;
        printConf.KaIds = configStatistic.ListKaId;
        printConf.TantoIds = configStatistic.ListTantoId;
        printConf.HokenSbts = configStatistic.ListHokenSbt;
        printConf.PtGrps = configStatistic.ListPtGrps;
        return printConf;
    }

    private CoSta3061PrintConf CreateCoSta3061PrintConf(ConfigStatistic3061Model configStatistic, int startDateFrom, int StartDateTo)
    {
        CoSta3061PrintConf printConf = new CoSta3061PrintConf(configStatistic.MenuId);
        printConf.FormFileName = configStatistic.FormReport;
        printConf.ReportName = configStatistic.ReportName;
        printConf.ReportKbn = configStatistic.ReportKbn;
        printConf.IsTester = configStatistic.TestPatient == 1;

        if (startDateFrom.AsString().Length == 8 && StartDateTo.AsString().Length == 8)
        {
            printConf.StartSinDate = startDateFrom;
            printConf.EndSinDate = StartDateTo;
        }
        else if (startDateFrom.AsString().Length == 6 && StartDateTo.AsString().Length == 6)
        {
            printConf.StartSinYm = startDateFrom;
            printConf.EndSinYm = StartDateTo;
        }

        printConf.PageBreak1 = configStatistic.PageBreak1;
        printConf.PageBreak2 = configStatistic.PageBreak2;
        printConf.PageBreak3 = configStatistic.PageBreak3;
        printConf.KaIds = configStatistic.ListKaId;
        printConf.TantoIds = configStatistic.ListTantoId;
        printConf.HokenSbts = configStatistic.ListHokenSbt;
        printConf.PtGrps = configStatistic.ListPtGrps;
        return printConf;
    }

    private CoSta3070PrintConf CreateCoSta3070PrintConf(ConfigStatistic3070Model configStatistic, int monthFrom, int monthTo)
    {
        CoSta3070PrintConf printConf = new CoSta3070PrintConf(configStatistic.MenuId);
        printConf.FormFileName = configStatistic.FormReport;
        printConf.ReportName = configStatistic.ReportName;
        printConf.ReportKbn = configStatistic.ReportKbn;
        printConf.IsTester = configStatistic.TestPatient == 1;
        printConf.RangeFrom = monthFrom;
        printConf.RangeTo = monthTo;

        printConf.PgBreak1 = configStatistic.PageBreak1;
        printConf.PgBreak2 = configStatistic.PageBreak2;
        printConf.PgBreak3 = configStatistic.PageBreak3;
        printConf.KaIds = configStatistic.ListKaId;
        printConf.TantoIds = configStatistic.ListTantoId;
        printConf.HokenSbts = configStatistic.ListHokenSbt;
        return printConf;
    }

    #endregion
}
