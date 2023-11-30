using Domain.Common;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta1001.Models;
using Reporting.Statistics.Sta2001.Models;

namespace Reporting.Statistics.Sta2001.DB;

public interface ICoSta2001Finder : IRepositoryBase
{
    List<CoSyunoInfModel> GetSyunoInfs(int hpId, CoSta2001PrintConf printConf);

    List<CoJihiSbtMstModel> GetJihiSbtMst(int hpId);

    List<CoJihiSbtFutan> GetJihiSbtFutan(int hpId, CoSta2001PrintConf printConf);

    CoHpInfModel GetHpInf(int hpId, int sinDate);
}
