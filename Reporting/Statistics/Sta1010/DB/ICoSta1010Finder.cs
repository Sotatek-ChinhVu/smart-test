using Domain.Common;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta1010.Models;

namespace Reporting.Statistics.Sta1010.DB;

public interface ICoSta1010Finder : IRepositoryBase
{
    List<CoSyunoInfModel> GetSyunoInfs(int hpId, CoSta1010PrintConf printConf);

    CoHpInfModel GetHpInf(int hpId, int sinDate);
}
