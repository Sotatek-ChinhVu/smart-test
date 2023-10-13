using Domain.Models.HpInf;
using Entity.Tenant;

namespace Reporting.KensaHistory.DB
{
    public interface ICoKensaHistoryFinder
    {
        HpInfModel GetHpInf(int hpId);

        PtInf GetPtInf(int hpId, long ptId);
    }
}
