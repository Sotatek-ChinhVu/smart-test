using Domain.Common;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3030.Models;

namespace Reporting.Statistics.Sta3030.DB;

public interface ICoSta3030Finder : IRepositoryBase
{
    List<CoPtByomeiModel> GetPtByomeiInfs(int hpId, CoSta3030PrintConf printConf);
    CoHpInfModel GetHpInf(int hpId, int sinDate);
}
