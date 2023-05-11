using Domain.Common;
using Entity.Tenant;
using Reporting.Kensalrai.Model;

namespace Reporting.Kensalrai.DB
{
    public interface ICoKensaIraiFinder : IRepositoryBase
    {
        List<CoRaiinInfModel> GetRaiinInf(int hpId, List<long> raiinNos);
        string GetContainerName(int hpId, long containerCd);
        (double height, double weight) GetHeightWeight(int hpId, long ptId, int sinDate);
        string GetCenterName(int hpId, string CenterCd);
        KensaMst GetKensaMst(int hpId, string itemCd);
        RaiinInf GetRaiinInf(int hpId, long raiinno);
        PtInf GetPtInf(int hpId, long ptid);
        List<KensaInfModel> GetKensaInfModelsPrint(int hpId, int startDate, int endDate, string centerCd);

        List<KensaIraiModel> GetKensaIraiModelsForPrint(int hpId, List<KensaInfModel> kensaInfModels);
    }
}
