using Domain.Models.HpInf;

namespace Reporting.KensaHistory.DB
{
    public interface ICoKensaHistoryFinder
    {
        HpInfModel GetHpInf(int hpId);
    }
}
