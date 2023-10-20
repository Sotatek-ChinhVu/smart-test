using Domain.Models.HpInf;
using Entity.Tenant;
using Reporting.KensaHistory.Models;

namespace Reporting.KensaHistory.DB
{
    public interface ICoKensaHistoryFinder
    {
        HpInfModel GetHpInf(int hpId);

        PtInf GetPtInf(int hpId, long ptId);

        (List<CoKensaResultMultiModel>, List<long>) GetListKensaInfDetail(int hpId, int userId, long ptId, int setId, int iraiCd, int startDate, bool showAbnormalKbn, int itemQuantity);
    }
}
