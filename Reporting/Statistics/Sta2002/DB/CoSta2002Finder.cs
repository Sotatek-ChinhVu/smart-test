using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Statistics.DB;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta1001.DB;
using Reporting.Statistics.Sta1001.Models;
using Reporting.Statistics.Sta2002.Models;

namespace Reporting.Statistics.Sta2002.DB
{
    public class CoSta2002Finder : RepositoryBase, ICoSta2002Finder
    {
        private readonly ICoHpInfFinder _coHpInfFinder;
        private readonly ICoSta1001Finder _coSta1001Finder;

        public CoSta2002Finder(ITenantProvider tenantProvider, ICoHpInfFinder coHpInfFinder, ICoSta1001Finder coSta1001Finder) : base(tenantProvider)
        {
            _coHpInfFinder = coHpInfFinder;
            _coSta1001Finder = coSta1001Finder;
        }

        public void ReleaseResource()
        {
            _coHpInfFinder.ReleaseResource();
            _coSta1001Finder.ReleaseResource();
            DisposeDataContext();
        }

        public CoHpInfModel GetHpInf(int hpId, int sinDate)
        {
            return _coHpInfFinder.GetHpInf(hpId, sinDate);
        }

        public List<CoSyunoInfModel> GetSyunoInfs(int hpId, CoSta2002PrintConf printConf)
        {
            CoSta1001PrintConf wrkPrintConf = convertPrintConf(printConf);
            var wrkSyunoInfs = _coSta1001Finder.GetSyunoInfs(hpId, wrkPrintConf, 2);

            //免除分をすべて当日精算扱いにする
            wrkSyunoInfs.FindAll(s => s.NyukinKbn == 2 && s.NyukinDate != s.SinDate).ForEach(s => s.NyukinDate = s.SinDate);

            return wrkSyunoInfs;
        }

        public List<CoJihiSbtFutan> GetJihiSbtFutan(int hpId, CoSta2002PrintConf printConf)
        {
            CoSta1001PrintConf wrkPrintConf = convertPrintConf(printConf);
            return _coSta1001Finder.GetJihiSbtFutan(hpId, wrkPrintConf);
        }

        public List<CoJihiSbtMstModel> GetJihiSbtMst(int hpId)
        {
            return _coSta1001Finder.GetJihiSbtMst(hpId);
        }

        private CoSta1001PrintConf convertPrintConf(CoSta2002PrintConf printConf)
        {
            return
                new CoSta1001PrintConf(printConf.MenuId)
                {
                    StartNyukinDate = printConf.StartNyukinYm * 100 + 1,
                    EndNyukinDate = printConf.EndNyukinYm * 100 + 31,
                    KaIds = printConf.KaIds,
                    TantoIds = printConf.TantoIds,
                    IsTester = printConf.IsTester,
                    IsExcludeUnpaid = printConf.IsExcludeUnpaid
                };
        }
    }
}
