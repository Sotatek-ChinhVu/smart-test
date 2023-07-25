using Domain.Common;
using Domain.Models.Accounting;
using Domain.Models.CalculationInf;
using Domain.Models.Diseases;
using Domain.Models.Medical;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.Receipt;
using Domain.Models.Receipt.Recalculation;
using Domain.Models.ReceSeikyu;
using Domain.Models.TodayOdr;

namespace Domain.CalculationInf
{
    public interface ICalculationInfRepository : IRepositoryBase
    {
        IEnumerable<CalculationInfModel> GetListDataCalculationInf(int hpId, long ptId);

        int GetCountReceInfs(int hpId, List<long> ptIds, int sinYm);

        List<ReceCheckOptModel> GetReceCheckOpts(int hpId);

        List<ReceInfModel> GetReceInfModels(int hpId, List<long> ptIds, int sinYM);

        List<SinKouiCountModel> GetSinKouiCounts(int hpId, long ptId, int sinYm, int hokenId);

        List<PtDiseaseModel> GetByomeiInThisMonth(int hpId, int sinYm, long ptId, int hokenId);

        List<OrdInfDetailModel> GetOdrInfsBySinDate(int hpId, long ptId, int sinDate, int hokenId);

        List<BuiOdrItemMstModel> GetBuiOdrItemMsts(int hpId);

        List<BuiOdrItemByomeiMstModel> GetBuiOdrItemByomeiMsts(int hpId);

        List<OrdInfModel> GetOdrInfModels(int hpId, long ptId, int sinYm, int hokenId);

        List<BuiOdrMstModel> GetBuiOdrMsts(int hpId);

        List<BuiOdrByomeiMstModel> GetBuiOdrByomeiMsts(int hpId);

        string GetSanteiItemCd(int hpId, string itemCd, int sinDate);

        List<string> GetTekiouByomei(int hpId, List<string> itemCds);

        int GetFirstVisitWithSyosin(int hpId, long ptId, int sinDate);

        TenItemModel? FindLastTenMst(int hpId, string itemCd);

        TenItemModel? FindFirstTenMst(int hpId, string itemCd);

        List<DensiSanteiKaisuModel> FindDensiSanteiKaisuList(int hpId, int sinDate, string itemCd);

        List<ItemGrpMstModel> FindItemGrpMst(int hpId, int sinDate, int grpSbt, long itemGrpCd);

        double SanteiCount(int hpId, long ptId, int startDate, int endDate, int sinDate, long raiinNo, List<string> itemCds, List<int> santeiKbns, List<int> hokenKbns);

        List<SinKouiModel> GetListSinKoui(int hpId, long ptId, int sinYm, int hokenId);

        List<string> GetListReceCmtItemCode(int hpId, long ptId, int sinYm, int hokenId);

        List<CalcLogModel> GetAddtionItems(int hpId, long ptId, int sinYm, int hokenId);

        bool IsKantokuCdValid(int hpId, int hokenId, long ptId);

        bool ExistSyobyoKeikaData(int hpId, long ptId, int sinYm, int hokenId);

        void ClearReceCmtErr(int hpId, long ptId, int hokenId, int sinYm);

        List<ReceSeikyuModel> GetReceSeikyus(int hpId, List<long> ptIds, int seikyuYm);

        List<TenItemModel> GetZaiganIsoItems(int hpId, int seikyuYm);

        List<SinKouiDetailModel> GetKouiDetailToCheckSantei(int hpId, List<long> ptIds, int seikyuYm, List<string> zaiganIsoItemCds, bool isCheckPartOfNextMonth);

        int GetSanteiStartDate(int hpId, long ptId, int seikyuYm);

        bool HasErrorWithSanteiByEndDate(int hpId, long ptId, int seikyuYm, int endDate, string itemCd);

        bool HasErrorWithSanteiByStartDate(int hpId, long ptId, int seikyuYm, int startDate, string itemCd);

        int GetSanteiEndDate(int hpId, long ptId, int seikyuYm);

        bool SaveChanged(int hpId, int userId, List<ReceCheckErrModel> receChecks);

        bool DeleteReceiptInfEdit(int hpId, int userId, int seikyuYm, long ptId, int sinYm, int hokenId);
    }
}
