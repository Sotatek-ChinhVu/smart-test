using Domain.Models.MstItem;
using Domain.Models.ReceptionSameVisit;

namespace Domain.Models.Accounting
{
    public interface IAccountingRepository
    {
        public List<AccountingModel> GetListSyunoSeikyu(int hpId, long ptId, int sinDate, long raiinNo, bool getAll = false);
        public List<RaiinInfModel> GetListRaiinInf(int hpId, long ptId, int sinDate, long raiinNo);
        public List<HokenPatternModel> FindPtHokenPatternList(int hpId, long ptId, int sinDay, List<int> listPatternId);
        public List<CalcLogModel> GetCalcLog(int hpId, long ptId, int sinDate, List<long> raiinNoList);
        public List<PtByomeiModel> GetPtByoMeiList(int hpId, long ptId, int sinDate = 0);
        public List<PaymentMethodMstModel> GetListPaymentMethodMst(int hpId);
    }
}
