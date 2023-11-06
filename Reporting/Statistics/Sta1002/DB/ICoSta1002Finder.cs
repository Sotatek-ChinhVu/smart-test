using Domain.Common;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta1001.Models;
using Reporting.Statistics.Sta1002.Models;

namespace Reporting.Statistics.Sta1002.DB;

public interface ICoSta1002Finder : IRepositoryBase
{
    List<CoSyunoInfModel> GetSyunoInfs(int hpId, CoSta1002PrintConf printConf);

    List<CoJihiSbtMstModel> GetJihiSbtMst(int hpId);

    List<CoJihiSbtFutan> GetJihiSbtFutan(int hpId, CoSta1002PrintConf printConf);

    CoHpInfModel GetHpInf(int hpId, int sinDate);
}
