using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3050.Models;

namespace Reporting.Statistics.Sta3050.DB;

public interface ICoSta3050Finder
{
    List<CoSinKouiModel> GetSinKouis(int hpId, CoSta3050PrintConf printConf);

    CoHpInfModel GetHpInf(int hpId, int sinDate);
}
