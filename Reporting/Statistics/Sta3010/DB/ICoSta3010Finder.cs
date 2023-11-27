using Domain.Common;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3010.Models;

namespace Reporting.Statistics.Sta3010.DB;

public interface ICoSta3010Finder : IRepositoryBase
{
    List<CoOdrSetModel> GetOdrSet(int hpId, CoSta3010PrintConf printConf);

    CoHpInfModel GetHpInf(int hpId, int sinDate);
}
