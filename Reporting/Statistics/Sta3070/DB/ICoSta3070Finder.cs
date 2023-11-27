using Domain.Common;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3070.Models;

namespace Reporting.Statistics.Sta3070.DB;

public interface ICoSta3070Finder : IRepositoryBase
{
    List<CoRaiinInfModel> GetRaiinInfs(int hpId, CoSta3070PrintConf printConf);

    CoHpInfModel GetHpInf(int hpId, int sinDate);
}
