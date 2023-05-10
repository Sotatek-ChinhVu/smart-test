using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3020.Models;

namespace Reporting.Statistics.Sta3020.DB
{
    public interface ICoSta3020Finder
    {
        List<CoListSetModel> GetListSet(int hpId, CoSta3020PrintConf printConf);

        CoHpInfModel GetHpInf(int hpId, int sinDate);
    }
}
