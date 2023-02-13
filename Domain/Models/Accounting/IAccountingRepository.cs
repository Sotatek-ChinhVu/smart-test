using Domain.Common;
using Domain.Models.Diseases;
using Domain.Models.MstItem;
using Domain.Models.Reception;
using Domain.Models.ReceptionSameVisit;

namespace Domain.Models.Accounting
{
    public interface IAccountingRepository : IRepositoryBase
    {
        List<AccountingModel> GetListSyunoSeikyu(int hpId, long ptId, int sinDate, long raiinNo, bool getAll = false);
        List<ReceptionDto> GetListRaiinInf(int hpId, long ptId, int sinDate, long raiinNo);
        List<HokenPatternModel> FindPtHokenPatternList(int hpId, long ptId, int sinDay, List<int> listPatternId);
        List<CalcLogModel> GetCalcLog(int hpId, long ptId, int sinDate, List<long> raiinNoList);
        List<PtDiseaseModel> GetPtByoMeiList(int hpId, long ptId, int sinDate = 0);
        List<PaymentMethodMstModel> GetListPaymentMethodMst(int hpId);
    }
}
