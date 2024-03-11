using Reporting.Statistics.Sta3062.Models;
using Reporting.Statistics.Model;
using Domain.Common;

namespace Reporting.Statistics.Sta3062.DB;

public interface ICoSta3062Finder : IRepositoryBase
{
    List<CoKouiTensuModel> GetKouiTensu(int hpId, CoSta3062PrintConf printConf);

    CoHpInfModel GetHpInf(int hpId, int sinDate);
}
