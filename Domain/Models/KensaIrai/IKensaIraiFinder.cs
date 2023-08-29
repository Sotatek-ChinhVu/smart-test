using Domain.Common;

namespace Domain.Models.KensaIrai;

public interface IKensaIraiRepository : IRepositoryBase
{
    KensaCenterMstModel GetKensaCenterMst(int hpId, string centerCd);

    List<KensaInfModel> GetKensaInf(int hpId, long ptId, long raiinNo, string centerCd);

    List<KensaInfDetailModel> GetKensaInfDetail(int hpId, long ptId, long raiinNo, string centerCd);
}
