using Domain.Common;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3060.Models;

namespace Reporting.Statistics.Sta3060.DB;

public interface ICoSta3060Finder : IRepositoryBase
{
    List<CoKouiTensuModel> GetKouiTensu(int hpId, CoSta3060PrintConf printConf);

    CoHpInfModel GetHpInf(int hpId, int sinDate);
}
