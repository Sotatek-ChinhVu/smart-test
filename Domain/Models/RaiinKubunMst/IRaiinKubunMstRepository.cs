using Domain.Common;

namespace Domain.Models.RaiinKubunMst
{
    public interface IRaiinKubunMstRepository : IRepositoryBase
    {
        List<RaiinKubunMstModel> GetList(bool isDeleted);

        List<RaiinKubunMstModel> LoadDataKubunSetting(int hpId, int userId);

        List<string> SaveDataKubunSetting(List<RaiinKubunMstModel> raiinKubunMstModels, int userId);

        List<(string, string)> GetListColumnName(int hpId);

        List<(int grpId, int kbnCd, int kouiKbn1, int kouiKbn2)> GetRaiinKouiKbns(int hpId);

        public List<RaiinKbnItemModel> GetRaiinKbnItems(int hpId);

    }
}
