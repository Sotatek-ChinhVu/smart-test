using Domain.Common;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta1001.Models;

namespace Reporting.Statistics.Sta1001.DB;

public interface ICoSta1001Finder : IRepositoryBase
{
    List<CoSyunoInfModel> GetSyunoInfs(int hpId, CoSta1001PrintConf printConf, int staMonthType);

    List<CoJihiSbtMstModel> GetJihiSbtMst(int hpId);

    List<CoJihiSbtFutan> GetJihiSbtFutan(int hpId, CoSta1001PrintConf printConf);

    CoHpInfModel GetHpInf(int hpId, int sinDate);

    string GetRaiinCmtInf(int hpId, long raiinNo);
}
