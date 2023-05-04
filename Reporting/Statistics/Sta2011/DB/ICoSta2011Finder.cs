using Reporting.Statistics.Model;
using Reporting.Statistics.Sta2010.Models;
using Reporting.Statistics.Sta2011.Models;

namespace Reporting.Statistics.Sta2011.DB
{
    public interface ICoSta2011Finder
    {
        List<CoReceInfModel> GetReceInfs(int hpId, CoSta2011PrintConf printConf, int prefNo);

        List<CoZaitakuModel> GetZaitakuReces(int hpId, CoSta2011PrintConf printConf);

        CoHpInfModel GetHpInf(int hpId, int sinDate);
    }
}
