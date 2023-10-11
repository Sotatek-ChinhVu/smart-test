using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3061.Models;

namespace Reporting.Statistics.Sta3061.DB;

public interface ICoSta3061Finder
{
    List<CoKouiTensuModel> GetKouiTensu(int hpId,CoSta3061PrintConf printConf);

    List<CoKouiTensuModel> GetKouiTensu2(int hpId,CoSta3061PrintConf printConf);

    CoHpInfModel GetHpInf(int hpId, int sinDate);

    List<CoJihiSbtMstModel> GetJihiSbtMst(int hpId);

    string GetPtGrpName(int hpId, int grpId);
}
