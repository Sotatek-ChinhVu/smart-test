using Domain.Common;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3041.Models;

namespace Reporting.Statistics.Sta3041.DB;

public interface ICoSta3041Finder : IRepositoryBase
{
    List<CoKouseisinInf> GetKouseisinInfs(int hpId, CoSta3041PrintConf printConf);

    CoHpInfModel GetHpInf(int hpId, int sinDate);
}
