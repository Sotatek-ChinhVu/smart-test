using Domain.Common;
using Reporting.AccountingCard.Model;

namespace Reporting.AccountingCard.DB
{
    public interface ICoAccountingCardFinder : IRepositoryBase
    {
        CoPtInfModel FindPtInf(int hpId, long ptId, int sinDate);
        List<CoKaikeiInfModel> FindKaikeiInf(int hpId, long ptId, int sinYm, int hokenId);
        List<CoPtByomeiModel> FindPtByomei(int hpId, long ptId, int startDate, int endDate, int hokenId);
    }
}
