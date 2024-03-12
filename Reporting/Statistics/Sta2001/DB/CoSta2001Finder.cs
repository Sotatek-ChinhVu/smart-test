using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Statistics.DB;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta1001.DB;
using Reporting.Statistics.Sta1001.Models;
using Reporting.Statistics.Sta2001.Models;

namespace Reporting.Statistics.Sta2001.DB;

public class CoSta2001Finder : RepositoryBase, ICoSta2001Finder
{
    private readonly ICoHpInfFinder _hpInfFinder;
    private readonly ICoSta1001Finder _sta1001Finder;
    public CoSta2001Finder(ITenantProvider tenantProvider, ICoHpInfFinder hpInfFinder, ICoSta1001Finder sta1001Finder) : base(tenantProvider)
    {
        _hpInfFinder = hpInfFinder;
        _sta1001Finder = sta1001Finder;
    }

    public void ReleaseResource()
    {
        _hpInfFinder.ReleaseResource();
        _sta1001Finder.ReleaseResource();
        DisposeDataContext();
    }

    public CoHpInfModel GetHpInf(int hpId, int sinDate)
    {
        return _hpInfFinder.GetHpInf(hpId, sinDate);
    }

    public List<CoSyunoInfModel> GetSyunoInfs(int hpId, CoSta2001PrintConf printConf)
    {
        CoSta1001PrintConf wrkPrintConf = convertPrintConf(printConf);
        var wrkSyunoInfs = _sta1001Finder.GetSyunoInfs(hpId, wrkPrintConf, 1);

        //免除分をすべて当日精算扱いにする
        wrkSyunoInfs.FindAll(s => s.NyukinKbn == 2 && s.NyukinDate != s.SinDate).ForEach(s => s.NyukinDate = s.SinDate);
        wrkSyunoInfs.RemoveAll(s => s.NyukinKbn == 2 && (s.NyukinDate < wrkPrintConf.StartNyukinDate || wrkPrintConf.EndNyukinDate < s.NyukinDate));

        return wrkSyunoInfs;
    }

    public List<CoJihiSbtFutan> GetJihiSbtFutan(int hpId, CoSta2001PrintConf printConf)
    {
        CoSta1001PrintConf wrkPrintConf = convertPrintConf(printConf);
        return _sta1001Finder.GetJihiSbtFutan(hpId, wrkPrintConf);
    }

    public List<CoJihiSbtMstModel> GetJihiSbtMst(int hpId)
    {
        return _sta1001Finder.GetJihiSbtMst(hpId);
    }

    private CoSta1001PrintConf convertPrintConf(CoSta2001PrintConf printConf)
    {
        return
            new CoSta1001PrintConf(printConf.MenuId)
            {
                StartNyukinDate = printConf.StartNyukinYm * 100 + 1,
                EndNyukinDate = printConf.EndNyukinYm * 100 + 31,
                KaIds = printConf.KaIds,
                TantoIds = printConf.TantoIds,
                PaymentMethodCds = printConf.PaymentMethodCds,
                IsTester = printConf.IsTester,
                IsExcludeUnpaid = printConf.IsExcludeUnpaid
            };
    }
}
