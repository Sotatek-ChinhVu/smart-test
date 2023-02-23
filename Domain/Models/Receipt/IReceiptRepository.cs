using Domain.Common;
using Domain.Models.Receipt.Recalculation;
using Domain.Models.Receipt.ReceiptListAdvancedSearch;

namespace Domain.Models.Receipt;

public interface IReceiptRepository : IRepositoryBase
{
    List<ReceiptListModel> GetReceiptList(int hpId, int seikyuYm, ReceiptListAdvancedSearchInput searchModel);

    List<ReceCmtModel> GetReceCmtList(int hpId, int sinYm, long ptId, int hokenId);

    bool SaveReceCmtList(int hpId, int userId, List<ReceCmtModel> receCmtList);

    List<SyoukiInfModel> GetSyoukiInfList(int hpId, int sinYm, long ptId, int hokenId);

    List<SyobyoKeikaModel> GetSyobyoKeikaList(int hpId, int sinYm, long ptId, int hokenId);

    List<ReceReasonModel> GetReceReasonList(int hpId, int seikyuYm, int sinDate, long ptId, int hokenId);

    List<ReceCheckCmtModel> GetReceCheckCmtList(int hpId, int sinYm, long ptId, int hokenId);

    List<ReceCheckErrModel> GetReceCheckErrList(int hpId, int sinYm, long ptId, int hokenId);

    List<SyoukiKbnMstModel> GetSyoukiKbnMstList(int sinYm);

    bool CheckExistSyoukiKbn(int sinYm, List<SyoukiKbnMstModel> syoukiKbnList);

    bool SaveSyoukiInfList(int hpId, int userId, List<SyoukiInfModel> syoukiInfList);

    bool SaveSyobyoKeikaList(int hpId, int userId, List<SyobyoKeikaModel> syoukiInfList);

    bool SaveReceCheckCmtList(int hpId, int userId, int hokenId, int sinYm, long ptId, List<ReceCheckCmtModel> receCheckCmtList);

    bool CheckExistSeqNoReceCheckCmtList(int hpId, int hokenId, int sinYm, long ptId, List<int> seqNoList);

    InsuranceReceInfModel GetInsuranceReceInfList(int hpId, int seikyuYm, int hokenId, int sinYm, long ptId);

    #region ReceRecalculation
    List<ReceRecalculationModel> GetReceRecalculationList(int hpId, int sinYm, List<long> ptIdList);

    List<SinKouiCountModel> GetSinKouiCountList(int hpId, int sinYm, long ptId, int hokenId);

    List<ReceCheckOptModel> GetReceCheckOptList(int hpId);

    bool ClearReceCmtErr(int hpId, long ptId, int hokenId, int sinYm);
    #endregion
}
