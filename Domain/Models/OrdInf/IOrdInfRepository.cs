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

        IEnumerable<OrdInfModel> GetList(int hpId, long ptId, long raiinNo, int sinDate, bool isDeleted);

        IEnumerable<OrdInfModel> GetList(long ptId, int hpId, long raiinNo);

        bool CheckExistOrder(long rpNo, long rpEdaNo);

        bool CheckIsGetYakkaPrice(int hpId, TenItemModel? tenMst, int sinDate);

        IpnMinYakkaMstModel FindIpnMinYakkaMst(int hpId, string ipnNameCd, int sinDate);
    }
}
