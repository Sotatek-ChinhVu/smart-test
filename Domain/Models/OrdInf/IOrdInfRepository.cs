﻿using Domain.Common;
using Domain.Models.MstItem;
using Domain.Models.OrdInf;
using Domain.Models.OrdInfDetails;
using Domain.Types;

namespace Domain.Models.OrdInfs
{
    public interface IOrdInfRepository : IRepositoryBase
    {
        void Create(OrdInfModel ord);

        OrdInfModel Read(int ordId);

        void Update(OrdInfModel ord);

        void Delete(int ordId);

        bool CheckExistOrder(int hpId, long ptId, long raiinNo, int sinDate, long rpNo, long rpEdaNo);

        bool CheckIsGetYakkaPrice(TenItemModel tenMst, int sinDate);

        IEnumerable<OrdInfModel> GetList(int hpId, long ptId, int userId, long raiinNo, int sinDate, bool isDeleted);

        IEnumerable<OrdInfModel> GetList(long ptId, int hpId, int userId, int deleteCondition, List<long> raiinNos);

        IEnumerable<OrdInfModel> GetList(long ptId, int hpId);

        List<OrdInfModel> GetList(int hpId, long ptId, int sinYm, int hokenPId);

        List<OrdInfDetailModel> GetOdrInfsBySinDate(int hpId, long ptId, int sinDate, int hokenPId);

        int GetSinDate(long ptId, int hpId, int searchType, int sinDate, List<long> listRaiiNoSameSinDate, string searchText);

        IpnMinYakkaMstModel FindIpnMinYakkaMst(int hpId, string ipnNameCd, int sinDate);

        IEnumerable<ApproveInfModel> GetApproveInf(int hpId, long ptId, bool isDeleted, List<long> raiinNos);

        long GetMaxRpNo(int hpId, long ptId, long raiinNo, int sinDate);

        IEnumerable<OrdInfModel> GetListToCheckValidate(long ptId, int hpId, List<long> raiinNos);

        List<IpnMinYakkaMstModel> GetCheckIpnMinYakkaMsts(int hpId, int sinDate, List<string> ipnNameCds);

        List<Tuple<string, string, bool>> CheckIsGetYakkaPrices(int hpId, List<TenItemModel> tenMsts, int sinDate);

        OrdInfModel GetHeaderInfo(int hpId, long ptId, long raiinNo, int sinDate);

        List<Tuple<string, string>> GetIpnMst(int hpId, int sinDateMin, int sinDateMax, List<string> ipnCds);

        bool CheckOrdInfInDrug(int hpId, long ptId, long raiinNo);

        List<OrdInfModel> GetIngaiKensaOdrInf(int hpId, long ptId, int sinDate, long raiinNo);

        List<OrdInfDetailModel> GetIngaiKensaOdrInfDetail(int hpId, long ptId, int sinDate, long raiinNo, string centerCd, int primaryKbn);
    }
}
