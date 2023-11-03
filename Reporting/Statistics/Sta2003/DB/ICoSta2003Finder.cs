using Domain.Common;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta1001.Models;
using Reporting.Statistics.Sta2003.Models;

namespace Reporting.Statistics.Sta2003.DB;

public interface ICoSta2003Finder : IRepositoryBase
{
    List<CoSyunoInfModel> GetSyunoInfs(int hpId, CoSta2003PrintConf printConf);

    List<CoJihiSbtMstModel> GetJihiSbtMst(int hpId);

    List<CoJihiSbtFutan> GetJihiSbtFutan(int hpId, CoSta2003PrintConf printConf);

    CoHpInfModel GetHpInf(int hpId, int sinDate);
}
