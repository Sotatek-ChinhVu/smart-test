using Domain.Common;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta2021.Models;

namespace Reporting.Statistics.Sta2021.DB
{
    public interface ICoSta2021Finder : IRepositoryBase
    {
        List<CoSinKouiModel> GetSinKouis(int hpId, CoSta2021PrintConf printConf);

        CoHpInfModel GetHpInf(int hpId, int sinYm);
    }
}
