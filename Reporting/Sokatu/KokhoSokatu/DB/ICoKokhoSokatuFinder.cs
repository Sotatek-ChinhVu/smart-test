using Reporting.Sokatu.Common.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.DB
{
    public interface ICoKokhoSokatuFinder
    {
        List<CoReceInfModel> GetReceInf(int hpId, int seikyuYm, SeikyuType seikyuType, KokhoKind kokhoKind, PrefKbn prefKbn, int prefNo, HokensyaNoKbn mainHokensyaNo);

        CoHpInfModel GetHpInf(int hpId, int seikyuYm);

        List<CoKaMstModel> GetKaMst(int hpId);

        List<CoHokensyaMstModel> GetHokensyaName(int hpId, List<string> hokensyaNos);

        List<CoKohiHoubetuMstModel> GetKohiHoubetuMst(int hpId, int seikyuYm);

        void ReleaseResource();
    }
}
