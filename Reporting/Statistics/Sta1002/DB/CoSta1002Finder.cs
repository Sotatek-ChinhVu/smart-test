using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Statistics.DB;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta1001.DB;
using Reporting.Statistics.Sta1001.Models;
using Reporting.Statistics.Sta1002.Models;

namespace Reporting.Statistics.Sta1002.DB;

public class CoSta1002Finder : RepositoryBase, ICoSta1002Finder
{
    private readonly ICoHpInfFinder _hpInfFinder;
    private readonly ICoSta1001Finder _sta1001Finder;
    public CoSta1002Finder(ITenantProvider tenantProvider, ICoHpInfFinder hpInfFinder, ICoSta1001Finder sta1001Finder) : base(tenantProvider)
    {
        _hpInfFinder = hpInfFinder;
        _sta1001Finder = sta1001Finder;
    }

    public CoHpInfModel GetHpInf(int hpId, int sinDate)
    {
        return _hpInfFinder.GetHpInf(hpId, sinDate);
    }

    public List<CoSyunoInfModel> GetSyunoInfs(int hpId, CoSta1002PrintConf printConf)
    {
        CoSta1001PrintConf wrkPrintConf = ConvertPrintConf(printConf);
        return _sta1001Finder.GetSyunoInfs(hpId, wrkPrintConf, 0);
    }

    public List<CoJihiSbtFutan> GetJihiSbtFutan(int hpId, CoSta1002PrintConf printConf)
    {
        CoSta1001PrintConf wrkPrintConf = ConvertPrintConf(printConf);
        return _sta1001Finder.GetJihiSbtFutan(hpId, wrkPrintConf);
    }

    public List<CoJihiSbtMstModel> GetJihiSbtMst(int hpId)
    {
        return _sta1001Finder.GetJihiSbtMst(hpId);
    }

    private CoSta1001PrintConf ConvertPrintConf(CoSta1002PrintConf printConf)
    {
        return
            new CoSta1001PrintConf(printConf.MenuId)
            {
                StartNyukinDate = printConf.StartNyukinDate,
                EndNyukinDate = printConf.EndNyukinDate,
                StartNyukinTime = printConf.StartNyukinTime,
                EndNyukinTime = printConf.EndNyukinTime,
                UketukeSbtIds = printConf.UketukeSbtIds,
                KaIds = printConf.KaIds,
                TantoIds = printConf.TantoIds,
                PaymentMethodCds = printConf.PaymentMethodCds,
                IsTester = printConf.IsTester,
                IsExcludeUnpaid = printConf.IsExcludeUnpaid
            };
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
