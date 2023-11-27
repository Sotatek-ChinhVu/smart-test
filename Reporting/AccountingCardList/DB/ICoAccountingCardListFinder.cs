using Domain.Common;
using Reporting.AccountingCardList.Model;

namespace Reporting.AccountingCardList.DB;

public interface ICoAccountingCardListFinder : IRepositoryBase
{
    CoPtInfModel FindPtInf(int hpId, long ptId, int sinDate);

    List<CoKaikeiInfModel> FindKaikeiInf(int hpId, long ptId, int sinYm, int hokenId);

    List<CoPtByomeiModel> FindPtByomei(int hpId, long ptId, int startDate, int endDate, int hokenId);
}
