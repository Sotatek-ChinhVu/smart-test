using Domain.Common;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3040.Models;

namespace Reporting.Statistics.Sta3040.DB;

public interface ICoSta3040Finder : IRepositoryBase
{
    List<CoUsedDrugInf> GetUsedDrugInfs(int hpId, CoSta3040PrintConf printConf);

    CoHpInfModel GetHpInf(int hpId, int sinDate);
}
