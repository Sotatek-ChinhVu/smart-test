using Domain.Common;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta2010.Models;

namespace Reporting.Statistics.Sta2010.DB
{
    public interface ICoSta2010Finder : IRepositoryBase
    {
        List<CoReceInfModel> GetReceInfs(int hpId, CoSta2010PrintConf printConf, int prefNo);

        List<CoKohiHoubetuMstModel> GetKohiHoubetuMst(int hpId, int seikyuYm);

        List<CoHokensyaMstModel> GetHokensyaName(int hpId, List<string> hokensyaNos);

        CoHpInfModel GetHpInf(int hpId, int sinDate);
    }
}
