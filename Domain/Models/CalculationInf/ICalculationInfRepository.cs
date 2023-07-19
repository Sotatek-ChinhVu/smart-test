using Domain.Common;
using Domain.Models.CalculationInf;
using Domain.Models.Diseases;
using Domain.Models.Medical;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.Receipt;
using Domain.Models.Receipt.Recalculation;

namespace Domain.CalculationInf
{
    public interface ICalculationInfRepository : IRepositoryBase
    {
        IEnumerable<CalculationInfModel> GetListDataCalculationInf(int hpId, long ptId);

        void CheckErrorInMonth(int hpId, int seikyuYm, List<long> ptIds);

        int GetCountReceInfs(int hpId, List<long> ptIds, int sinYm);

        List<ReceCheckOptModel> GetReceCheckOpts(int hpId);

        List<ReceInfModel> GetReceInfModels(int hpId, List<long> ptIds, int sinYM);

        List<SinKouiCountModel> GetSinKouiCounts(int hpId, long ptId, int sinYm, int hokenId);

        void InsertReceCmtErr(int hpId, int userId, string userName, List<ReceCheckErrModel> oldReceCheckErrs, List<ReceCheckErrModel> newReceCheckErrs, ReceInfModel receInfModel, string errCd, string errMsg1, string errMsg2 = "", string aCd = " ", string bCd = " ", int sinDate = 0);

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
    }
}
