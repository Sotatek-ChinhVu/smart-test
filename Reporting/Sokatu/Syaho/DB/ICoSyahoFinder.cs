using Reporting.Sokatu.Common.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.Syaho.DB;

public interface ICoSyahoFinder
{
    List<CoReceInfModel> GetReceInf(int hpId, int seikyuYm, SeikyuType seikyuType);

    CoHpInfModel GetHpInf(int hpId, int seikyuYm);
}
