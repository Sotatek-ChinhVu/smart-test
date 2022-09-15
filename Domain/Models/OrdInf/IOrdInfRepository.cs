using Domain.Models.MstItem;
using Domain.Models.OrdInf;

namespace Domain.Models.OrdInfs
{
    public interface IOrdInfRepository
    {
        void Create(OrdInfModel ord);

        OrdInfModel Read(int ordId);

        void Update(OrdInfModel ord);

        void Delete(int ordId);

        bool CheckExistOrder(long rpNo, long rpEdaNo);

        bool CheckIsGetYakkaPrice(int hpId, TenItemModel tenMst, int sinDate);

        IEnumerable<OrdInfModel> GetList(int hpId, long ptId, int userId, long raiinNo, int sinDate, bool isDeleted);

        IEnumerable<OrdInfModel> GetList(long ptId, int hpId, int userId, int deleteCondition, List<long> raiinNos);

        long GetRaiinNo(long ptId, int hpId, int searchType, long raiinNo, string searchText);

        IpnMinYakkaMstModel FindIpnMinYakkaMst(int hpId, string ipnNameCd, int sinDate);

        IEnumerable<ApproveInfModel> GetApproveInf(int hpId, long ptId, bool isDeleted, List<long> raiinNos);
    }
}
