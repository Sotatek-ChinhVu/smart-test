﻿using Domain.Common;
using Domain.Models.Accounting;
using Domain.Models.OrdInfDetails;
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

    List<SyobyoKeikaModel> GetSyobyoKeikaList(int hpId, List<int> sinYmList, List<long> ptIdList, List<int> hokenIdList);

    List<ReceReasonModel> GetReceReasonList(int hpId, int seikyuYm, int sinDate, long ptId, int hokenId);

    List<ReceCheckCmtModel> GetReceCheckCmtList(int hpId, int sinYm, long ptId, int hokenId);

    List<ReceCheckErrModel> GetReceCheckErrList(int hpId, int sinYm, long ptId, int hokenId);

    List<ReceCheckErrModel> GetReceCheckErrList(int hpId, List<int> sinYmList, List<long> ptIdList, List<int> hokenIdList);

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

    List<BuiOdrItemMstModel> GetBuiOdrItemMstList(int hpId);

    List<BuiOdrItemByomeiMstModel> GetBuiOdrItemByomeiMstList(int hpId);

    List<BuiOdrMstModel> GetBuiOdrMstList(int hpId);

    List<BuiOdrByomeiMstModel> GetBuiOdrByomeiMstList(int hpId);

    string GetSanteiItemCd(int hpId, string itemCd, int sinDate);

    List<string> GetTekiouByomei(int hpId, List<string> itemCdList);

    double SanteiCount(int hpId, long ptId, int startDate, int endDate, int sinDate, long raiinNo, List<string> itemCds, List<int> santeiKbns, List<int> hokenKbns);

    List<SinKouiMstModel> GetListSinKoui(int hpId, long ptId, int sinYm, int hokenId);

    List<string> GetListReceCmtItemCode(int hpId, long ptId, int sinYm, int hokenId);

    List<CalcLogModel> GetAddtionItems(int hpId, long ptId, int sinYm, int hokenId);

    bool SaveNewReceCheckErrList(int hpId, int userId, List<ReceCheckErrModel> receCheckErrList);
    #endregion
}
