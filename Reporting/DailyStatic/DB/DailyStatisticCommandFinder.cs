using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.DailyStatic.Enum;
using Reporting.DailyStatic.Model;

namespace Reporting.DailyStatic.DB;

public class DailyStatisticCommandFinder : RepositoryBase, IDailyStatisticCommandFinder
{
    public DailyStatisticCommandFinder(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    public ConfigStatisticModel GetDailyConfigStatisticMenu(int hpId, int menuId)
    {
        var staMenu = NoTrackingDataContext.StaMenus.Where(x => x.HpId == hpId && x.MenuId == menuId)
                                                 .FirstOrDefault();
        if (staMenu != null)
        {
            var listConfig = NoTrackingDataContext.StaConfs.Where(x => x.HpId == hpId && x.MenuId == menuId)
                                                        .ToList();

            switch (staMenu.ReportId)
            {
                case (int)StatisticReportType.Sta2021:
                    return new ConfigStatisticModel(new ConfigStatistic2021Model(staMenu, listConfig));
                case (int)StatisticReportType.Sta3020:
                    return new ConfigStatisticModel(new ConfigStatistic3020Model(staMenu, listConfig));
                case (int)StatisticReportType.Sta3030:
                    return new ConfigStatisticModel(new ConfigStatistic3030Model(staMenu, listConfig));
                case (int)StatisticReportType.Sta3040:
                    return new ConfigStatisticModel(new ConfigStatistic3040Model(staMenu, listConfig));
                case (int)StatisticReportType.Sta3041:
                    return new ConfigStatisticModel(new ConfigStatistic3041Model(staMenu, listConfig));
                case (int)StatisticReportType.Sta3050:
                    return new ConfigStatisticModel(new ConfigStatistic3050Model(staMenu, listConfig));
                case (int)StatisticReportType.Sta3060:
                    return new ConfigStatisticModel(new ConfigStatistic3060Model(staMenu, listConfig));
                case (int)StatisticReportType.Sta3061:
                    return new ConfigStatisticModel(new ConfigStatistic3061Model(staMenu, listConfig));
                case (int)StatisticReportType.Sta3070:
                    return new ConfigStatisticModel(new ConfigStatistic3070Model(staMenu, listConfig));
                case (int)StatisticReportType.Sta3071:
                    return new ConfigStatisticModel(new ConfigStatistic3071Model(staMenu, listConfig));
                case (int)StatisticReportType.Sta3080:
                    return new ConfigStatisticModel(new ConfigStatistic3080Model(staMenu, listConfig));
                default:
                    return new ConfigStatisticModel(staMenu, listConfig);
            }
        }
        return new(0);
    }
}
