using Domain.Common;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3080.Models;

namespace Reporting.Statistics.Sta3080.DB;

public interface ICoSta3080Finder : IRepositoryBase
{
    List<CoSeisinDayCareInf> GetSeisinDayCareInfs(int hpId, CoSta3080PrintConf printConf);
    CoHpInfModel GetHpInf(int hpId, int sinDate);
}
