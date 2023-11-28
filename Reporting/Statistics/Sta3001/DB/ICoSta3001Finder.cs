using Domain.Common;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3001.Models;

namespace Reporting.Statistics.Sta3001.DB
{
    public interface ICoSta3001Finder : IRepositoryBase
    {
        List<CoAdpDrugsModel> GetAdpDrugs(int hpId, CoSta3001PrintConf printConf);

        CoHpInfModel GetHpInf(int hpId, int sinDate);
    }
}
