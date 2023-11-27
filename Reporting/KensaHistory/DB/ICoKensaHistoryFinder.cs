using Domain.Common;
using Domain.Models.HpInf;
using Domain.Models.KensaIrai;
using Entity.Tenant;
using Reporting.KensaHistory.Models;

namespace Reporting.KensaHistory.DB
{
    public interface ICoKensaHistoryFinder : IRepositoryBase
    {
        HpInfModel GetHpInf(int hpId, int sinDate);

        PtInf GetPtInf(int hpId, long ptId);

        (List<CoKensaResultMultiModel>, List<long>) GetListKensaInfDetail(int hpId, int userId, long ptId, int setId, int startDate, int endDate, bool showAbnormalKbn);

        ListKensaInfDetailModel GetListKensaInf(int hpId, int userId, long ptId, int setId, int iraiCdStart, bool getGetPrevious, bool showAbnormalKbn, int startDate = 0);
    }
}
