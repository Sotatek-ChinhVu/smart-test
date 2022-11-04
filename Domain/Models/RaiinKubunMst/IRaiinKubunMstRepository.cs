namespace Domain.Models.RaiinKubunMst
{
    public interface IRaiinKubunMstRepository
    {
        List<RaiinKubunMstModel> GetList(bool isDeleted);

        List<RaiinKubunMstModel> LoadDataKubunSetting(int hpId, int userId);

        List<string> SaveDataKubunSetting(List<RaiinKubunMstModel> raiinKubunMstModels, int userId);

        List<string> GetListColumnName(int hpId);
    }
}
