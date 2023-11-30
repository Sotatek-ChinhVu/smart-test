using Domain.Common;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3071.Models;

namespace Reporting.Statistics.Sta3071.DB;

public interface ICoSta3071Finder : IRepositoryBase
{
    List<CoRaiinInfModel> GetRaiinInfs(int hpId, CoSta3071PrintConf printConf);

    string GetPtGrpName(int hpId, int grpId);

    CoHpInfModel GetHpInf(int hpId, int sinDate);
}
