using Reporting.Statistics.Model;
using Reporting.Statistics.Sta1001.Models;

namespace Reporting.Statistics.Sta1001.DB
{
    public interface ICoSta1001Finder
    {
        CoHpInfModel GetHpInf(int hpId, int sinDate);
        List<CoSyunoInfModel> GetSyunoInfs(int hpId, CoSta1001PrintConf printConf, int staMonthType);
        List<CoJihiSbtFutan> GetJihiSbtFutan(int hpId, CoSta1001PrintConf printConf);
        List<CoJihiSbtMstModel> GetJihiSbtMst(int hpId);
        string GetRaiinCmtInf(int hpId, long raiinNo);
    }
}
