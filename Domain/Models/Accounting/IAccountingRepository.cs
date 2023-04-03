using Domain.Common;
using Domain.Models.AccountDue;
using Domain.Models.Diseases;
using Domain.Models.Insurance;
using Domain.Models.MstItem;
using Domain.Models.Reception;
using Domain.Models.ReceptionSameVisit;

namespace Domain.Models.Accounting
{
    public interface IAccountingRepository : IRepositoryBase
    {
        List<SyunoSeikyuModel> GetListSyunoSeikyu(int hpId, long ptId, int sinDate, List<long> raiinNo, bool getAll = false);
        List<ReceptionDto> GetListRaiinInf(int hpId, long ptId, int sinDate, long raiinNo, bool isGetHeader = false, bool getAll = true);
        List<HokenPatternModel> FindPtHokenPatternList(int hpId, long ptId, int sinDay, List<int> listPatternId);
        List<CalcLogModel> GetCalcLog(int hpId, long ptId, int sinDate, List<long> raiinNoList);
        List<PtDiseaseModel> GetPtByoMeiList(int hpId, long ptId, int sinDate = 0);
        List<PaymentMethodMstModel> GetListPaymentMethodMst(int hpId);
        List<KohiInfModel> GetListKohiByKohiId(int hpId, long ptId, int sinDate, List<int> listKohiId);
        bool SaveAccounting(List<SyunoSeikyuModel> listAllSyunoSeikyu, List<SyunoSeikyuModel> syunoSeikyuModels, int hpId, long ptId, int userId, int accDue, int sumAdjust, int thisWari, int thisCredit,
                                   int payType, string comment, bool isDisCharged);
        bool CheckRaiinInfExist(int hpId, long ptId, long raiinNo);
        List<long> GetRaiinNos(int hpId, long ptId, long raiinNo);
        void CheckOrdInfInOutDrug(int hpId, long ptId, List<long> raiinNos, out bool inDrugExist, out bool outDrugExist);
        List<JihiSbtMstModel> GetListJihiSbtMst(int hpId);
        int GetJihiOuttaxPoint(int hpId, long ptId, List<long> raiinNos);
        byte CheckIsOpenAccounting(int hpId, long ptId, int sinDate, long raiinNo);
        bool CheckSyunoStatus(int hpId, long raiinNo, long ptId);
    }
}
