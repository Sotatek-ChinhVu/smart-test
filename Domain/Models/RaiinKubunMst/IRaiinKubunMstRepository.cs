namespace Domain.Models.RaiinKubunMst
{
    public interface IRaiinKubunMstRepository
    {
        List<RaiinKubunMstModel> GetList(bool isDeleted);
        List<RaiinKubunMstModel> LoadDataKubunSetting(int HpId);
        List<(bool, string)> SaveDataKubunSetting(List<RaiinKubunMstModel> raiinKubunMstModels);
    }
}
